using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;
using tonkica.Enums;
using tonkica.Localization;
using tonkica.Models;
using tonkica.Services;

namespace tonkica.Pages
{
    public partial class InvoiceId
    {
        [Inject] private AppDbContext Db { get; set; } = null!;
        [Inject] private CurrencyRatesClient Rates { get; set; } = null!;
        [Inject] private ClockifyClient Clockify { get; set; } = null!;
        [Parameter] public int Id { get; set; }
        List<Currency> _currencies = new();
        Dictionary<int, string> _currenciesD = new();
        List<Issuer> _issuers = new();
        Dictionary<int, string> _issuersD = new();
        List<Client> _clients = new();
        Dictionary<int, string> _clientsD = new();
        readonly Dictionary<int, string> _statusesD = new();
        Issuer? _issuer;
        Invoice? _invoice;
        InvoiceEditModel? _edit;
        InvoiceItem _item = null!;
        bool _clockifyOpen;
        DateTimeOffset? _clockifyStart;
        DateTimeOffset? _clockifyEnd;
        List<InvoiceItem> _clockifyItems = new();
        Dictionary<string, string>? _errors;
        bool _isDraft;
        readonly IInvoice _t = LocalizationFactory.Invoice();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            foreach (var e in Enum.GetValues(typeof(InvoiceStatus)))
                _statusesD.Add((int)e, e.ToString()!);

            _currencies = await Db.Currencies.ToListAsync();
            _currenciesD = _currencies.ToDictionary(x => x.Id, x => x.Tag);

            _issuers = await Db.Issuers.ToListAsync();
            _issuersD = _issuers.ToDictionary(x => x.Id, x => x.Name);

            _clients = await Db.Clients.ToListAsync();
            _clientsD = _clients.ToDictionary(x => x.Id, x => x.Name);

            _invoice = await Db.Invoices
                .Include(x => x.Items)
                .Include(x => x.Account)
                .SingleOrDefaultAsync(x => x.Id == Id);

            if (_invoice != null)
            {
                _isDraft = _invoice.Status == (int)InvoiceStatus.Draft;
                _edit = new InvoiceEditModel(_invoice);
                _item = new InvoiceItem(string.Empty) { InvoiceId = _invoice.Id };
                _issuer = _issuers.Single(i => i.Id == _edit.IssuerId);
            }

            _clockifyEnd = DateTime.UtcNow;
            _clockifyStart = new DateTime(_clockifyEnd.Value.Year, _clockifyEnd.Value.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        }
        private async Task<EventCallback<EventArgs>> SaveClicked()
        {
            if (_edit == null || _invoice == null)
                return default;

            _errors = _edit!.Validate(_t);
            if (_errors != null)
                return default;

            _invoice.SequenceNumber = _edit.SequenceNumber;
            _invoice.Published = _edit.Published;
            _invoice.Status = (InvoiceStatus)_edit.Status;

            if (!_invoice.Published.HasValue && _invoice.Status == InvoiceStatus.Issued)
                _invoice.Published = DateTimeOffset.UtcNow;
            else if (_invoice.Published.HasValue && _invoice.Status == InvoiceStatus.Draft)
                _invoice.Published = null;

            var issuer = _issuers.Single(x => x.Id == _edit.IssuerId);
            var client = _clients.Single(x => x.Id == _edit.ClientId);

            _invoice.Subject = _edit.Subject!;
            _invoice.IssuerId = _edit.IssuerId;
            _invoice.ClientId = _edit.ClientId;
            _invoice.Note = _edit.Note;

            _invoice.Currency = _currencies.First(c => c.Id == _edit.CurrencyId);
            _invoice.DisplayCurrency = _currencies.First(c => c.Id == _edit.DisplayCurrencyId);
            _invoice.IssuerCurrency = _currencies.First(c => c.Id == _edit.IssuerCurrencyId);

            if (_edit.DisplayCurrencyId != _invoice.Account!.CurrencyId)
            {
                var newAccount = await Db.Accounts.FirstOrDefaultAsync(x => x.CurrencyId == _edit.DisplayCurrencyId);
                if (newAccount != null)
                    _invoice.Account = newAccount;
            }

            await Rates.CalculateRates(_invoice);
            CalculateTotals();

            await Db.SaveChangesAsync();
            _edit = new InvoiceEditModel(_invoice);
            _isDraft = _invoice.Status == (int)InvoiceStatus.Draft;
            return default;
        }
        private void CalculateTotals()
        {
            foreach (var item in _invoice!.Items)
                item.Total = item.Price * item.Quantity;

            _invoice.QuantityTotal = _invoice.Items.Sum(i => i.Quantity);
            _invoice.Total = _invoice.Items.Sum(i => i.Total);
            _invoice.DisplayTotal = _invoice.Total * _invoice.DisplayRate;
            _invoice.IssuerTotal = _invoice.Total * _invoice.IssuerRate;
        }
        private void CloseClockify() => _clockifyOpen = false;
        private void ClockifyOpen()
        {
            _clockifyItems.Clear();
            _clockifyOpen = true;
        }
        private async Task ClockifyLoad()
        {
            var errors = new Dictionary<string, string>();
            if (!_clockifyStart.HasValue)
                errors.Add(nameof(_clockifyStart), _t.ValidationRequired);
            if (!_clockifyEnd.HasValue)
                errors.Add(nameof(_clockifyEnd), _t.ValidationRequired);

            if (errors.Any())
            {
                _errors = errors;
                return;
            }

            var entries = await Clockify.GetDefaultTimeEntries(_invoice!.Issuer!.ClockifyUrl!, _clockifyStart!.Value.UtcDateTime, _clockifyEnd!.Value.UtcDateTime);
            var groups = entries.GroupBy(e => e.Description);

            foreach (var group in groups)
            {
                var ts = TimeSpan.FromSeconds(group.Sum(e => e.Interval.Duration));
                var item = new InvoiceItem(group.Key)
                {
                    InvoiceId = _invoice!.Id,
                    Price = _invoice?.Client?.ContractRate ?? 0,
                    Quantity = (decimal)ts.TotalHours,
                };
                _clockifyItems.Add(item);
            }
        }
        private void ClockifyAdd()
        {
            foreach (var item in _clockifyItems)
                _invoice!.Items?.Add(item);
        }
        private void ClearInvoiceItems()
        {
            if (_invoice == null)
                return;

            _invoice.Items.Clear();
            CalculateTotals();
        }

        private void AddItemClicked()
        {
            if (_invoice == null)
                return;

            _invoice.Items.Add(_item);
            CalculateTotals();
            _item = new InvoiceItem(string.Empty)
            {
                InvoiceId = _invoice.Id
            };
        }
        private void RemoveItemClicked(InvoiceItem item)
        {
            if (_invoice == null)
                return;

            _invoice.Items.Remove(item);
            CalculateTotals();
        }
    }
}