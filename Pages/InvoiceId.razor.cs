using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;
using tonkica.Enums;
using tonkica.Models;
using tonkica.Services;

namespace tonkica.Pages
{
    public partial class InvoiceId
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        [Inject] private CurrencyRatesClient _rates { get; set; } = null!;
        [Parameter] public int Id { get; set; }
        private IList<Currency> _currencies = new List<Currency>();
        private Dictionary<int, string> _currenciesD = new Dictionary<int, string>();
        private IList<Issuer> _issuers = new List<Issuer>();
        private Dictionary<int, string> _issuersD = new Dictionary<int, string>();
        private IList<Client> _clients = new List<Client>();
        private Dictionary<int, string> _clientsD = new Dictionary<int, string>();
        private Dictionary<int, string> _statusesD = new Dictionary<int, string>();
        private Invoice? _invoice;
        private InvoiceEditModel? _edit;
        private InvoiceItem _item = null!;
        private Dictionary<string, string>? _errors;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            foreach (var e in Enum.GetValues(typeof(InvoiceStatus)))
                _statusesD.Add((int)e, e.ToString()!);

            _currencies = await _db.Currencies.ToListAsync();
            _currenciesD = _currencies.ToDictionary(x => x.Id, x => x.Tag);

            _issuers = await _db.Issuers.ToListAsync();
            _issuersD = _issuers.ToDictionary(x => x.Id, x => x.Name);

            _clients = await _db.Clients.ToListAsync();
            _clientsD = _clients.ToDictionary(x => x.Id, x => x.Name);

            _invoice = await _db.Invoices
                .Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.Id == Id);

            if (_invoice != null)
            {
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

            var issuer = _issuers.Single(x => x.Id == _edit.IssuerId);
            var client = _clients.Single(x => x.Id == _edit.ClientId);

            _invoice.Subject = _edit.Subject!;
            _invoice.IssuerId = _edit.IssuerId;
            _invoice.ClientId = _edit.ClientId;
            _invoice.Note = _edit.Note;

            _invoice.Currency = client.ContractCurrency;
            _invoice.DisplayCurrency = client.DisplayCurrency;
            _invoice.IssuerCurrency = issuer.Currency;

            await _rates.CalculateRates(_invoice);

            await _db.SaveChangesAsync();

            return default;
        }
        private async Task<EventCallback<EventArgs>> SaveItemsClicked()
        {
            await SaveInvoiceItems();

            return default;
        }

        private async Task SaveInvoiceItems()
        {
            if (_invoice == null || _invoice.Items == null)
                return;

            foreach (var item in _invoice.Items)
                item.Total = item.Price * item.Quantity;

            _invoice.Total = _invoice.Items.Sum(i => i.Total);
            _invoice.DisplayTotal = _invoice.Total * _invoice.DisplayRate;
            _invoice.IssuerTotal = _invoice.Total * _invoice.IssuerRate;

            await _db.SaveChangesAsync();
        }

        private async Task<EventCallback<EventArgs>> AddItemClicked()
        {
            if (_invoice == null || _invoice.Items == null)
                return default;

            _invoice.Items.Add(_item);
            await SaveInvoiceItems();

            _item = new InvoiceItem(string.Empty)
            {
                InvoiceId = _invoice.Id
            };

            return default;
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