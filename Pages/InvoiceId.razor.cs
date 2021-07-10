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
        private List<Currency> _currencies = new();
        private Dictionary<int, string> _currenciesD = new();
        private List<Issuer> _issuers = new();
        private Dictionary<int, string> _issuersD = new();
        private List<Client> _clients = new();
        private Dictionary<int, string> _clientsD = new();
        private readonly Dictionary<int, string> _statusesD = new();
        private Invoice? _invoice;
        private InvoiceEditModel? _edit;
        private InvoiceItem _item = null!;
        private Dictionary<string, string>? _errors;
        private bool IsDraft { get; set; }
        private readonly IInvoice _t = LocalizationFactory.Invoice();

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
                IsDraft = _invoice.Status == (int)InvoiceStatus.Draft;
                _edit = new InvoiceEditModel(_invoice);
                _item = new InvoiceItem(string.Empty)
                {
                    InvoiceId = _invoice.Id
                };
            }
        }
        private async Task<EventCallback<EventArgs>> SaveClicked()
        {
            if (_edit == null || _invoice == null)
                return default;

            _errors = _edit!.Validate();
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

            _invoice.Total = _invoice.Items!.Sum(i => i.Total);
            _invoice.DisplayTotal = _invoice.Total * _invoice.DisplayRate;
            _invoice.IssuerTotal = _invoice.Total * _invoice.IssuerRate;

            await Db.SaveChangesAsync();
            _edit = new InvoiceEditModel(_invoice);
            IsDraft = _invoice.Status == (int)InvoiceStatus.Draft;
            return default;
        }
        private async Task ClockifyClicked()
        {
            if (string.IsNullOrWhiteSpace(_invoice?.Issuer?.ClockifyUrl))
                return;

            var entries = await Clockify.GetDefaultTimeEntries(_invoice.Issuer.ClockifyUrl);
            var groups = entries.GroupBy(e => e.Description);

            foreach (var group in groups)
            {
                var ts = TimeSpan.FromSeconds(group.Sum(e => e.Interval.Duration));
                var item = new InvoiceItem(group.Key)
                {
                    InvoiceId = _invoice.Id,
                    Price = _invoice?.Client?.ContractRate ?? 0,
                    Quantity = (decimal)ts.TotalHours,
                };
                _invoice!.Items?.Add(item);
            }

            await SaveInvoiceItems();
        }
        private async Task SaveInvoiceItems()
        {
            if (_invoice == null || _invoice.Items == null)
                return;

            foreach (var item in _invoice.Items)
                item.Total = item.Price * item.Quantity;

            _invoice.QuantityTotal = _invoice.Items.Sum(i => i.Quantity);
            _invoice.Total = _invoice.Items.Sum(i => i.Total);
            _invoice.DisplayTotal = _invoice.Total * _invoice.DisplayRate;
            _invoice.IssuerTotal = _invoice.Total * _invoice.IssuerRate;

            await Db.SaveChangesAsync();
        }

        private async Task AddItemClicked()
        {
            if (_invoice == null || _invoice.Items == null)
                return;

            _invoice.Items.Add(_item);
            await SaveInvoiceItems();

            _item = new InvoiceItem(string.Empty)
            {
                InvoiceId = _invoice.Id
            };
        }
        private async void RemoveItemClicked(InvoiceItem item)
        {
            if (_invoice == null || _invoice.Items == null)
                return;

            _invoice.Items.Remove(item);
            await SaveInvoiceItems();
        }
    }
}