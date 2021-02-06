using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;
using tonkica.Models;
using tonkica.Services;

namespace tonkica.Pages
{
    public partial class Invoices
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        [Inject] private CurrencyRatesClient _rates { get; set; } = null!;
        [Inject] private NavigationManager _navigator { get; set; } = null!;
        private IList<Currency> _currencies = new List<Currency>();
        private Dictionary<int, string> _currenciesD = new Dictionary<int, string>();
        private IList<Issuer> _issuers = new List<Issuer>();
        private Dictionary<int, string> _issuersD = new Dictionary<int, string>();
        private IList<Client> _clients = new List<Client>();
        private Dictionary<int, string> _clientsD = new Dictionary<int, string>();
        private List<Invoice> _list = new List<Invoice>();
        private InvoiceCreateModel? _create;
        private Dictionary<string, string>? _errors;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currencies = await _db.Currencies.ToListAsync();
            _currenciesD = _currencies.ToDictionary(x => x.Id, x => x.Tag);

            _issuers = await _db.Issuers.ToListAsync();
            _issuersD = _issuers.ToDictionary(x => x.Id, x => x.Name);

            _clients = await _db.Clients.ToListAsync();
            _clientsD = _clients.ToDictionary(x => x.Id, x => x.Name);

            _list = await _db.Invoices.ToListAsync();
        }
        private async Task AddClicked()
        {
            if (!_issuersD.Any())
                _issuersD = await _db.Issuers.ToDictionaryAsync(i => i.Id, i => i.Name);
            if (!_clientsD.Any())
                _clientsD = await _db.Clients.ToDictionaryAsync(i => i.Id, i => i.Name);

            _create = new InvoiceCreateModel();

            _create.IssuerId = _issuersD.FirstOrDefault().Key;
            _create.ClientId = _clientsD.FirstOrDefault().Key;
        }
        private void CancelClicked() { _create = null; _errors = null; }
        private async Task<EventCallback<EventArgs>> SaveClicked()
        {
            if (_create == null)
                return default;

            _errors = _create!.Validate();
            if (_errors != null)
                return default;

            var issuer = _issuers.Single(x => x.Id == _create.IssuerId);
            var client = _clients.Single(x => x.Id == _create.ClientId);

            var invoice = new Invoice();
            invoice.Subject = _create.Subject!;
            invoice.IssuerId = _create.IssuerId;
            invoice.ClientId = _create.ClientId;
            invoice.Note = _create.Note;

            invoice.Currency = client.ContractCurrency;
            invoice.DisplayCurrency = client.DisplayCurrency;
            invoice.IssuerCurrency = issuer.Currency;

            await _rates.CalculateRates(invoice);

            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync();
            _create = null;

            _navigator.NavigateTo(C.Routes.InvoiceFor(invoice.Id));
            return default;
        }
    }
}