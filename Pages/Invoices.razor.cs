using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;
using tonkica.Models;
using tonkica.Services;
using tonkica.Shared;

namespace tonkica.Pages
{
    public partial class Invoices
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        [Inject] private CurrencyRatesClient _rates { get; set; } = null!;
        [Inject] private NavigationManager _navigator { get; set; } = null!;
        [Inject] private NavigationManager _navManager { get; set; } = null!;
        private const string QUERY_YEAR = "year";
        private int _defaultYear = DateTime.UtcNow.Year;
        private int _currentYear;
        private const string QUERY_MONTH = "month";
        private int _defaultMonth = DateTime.UtcNow.Month;
        private int? _currentMonth;
        private QueryStepper? stepperYear { get; set; }
        private IList<Currency> _currencies = new List<Currency>();
        private Dictionary<int, string> _currenciesD = new Dictionary<int, string>();
        private IList<Issuer> _issuers = new List<Issuer>();
        private Dictionary<int, string> _issuersD = new Dictionary<int, string>();
        private IList<Client> _clients = new List<Client>();
        private Dictionary<int, string> _clientsD = new Dictionary<int, string>();
        private IList<Account> _accounts = new List<Account>();
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

            _accounts = await _db.Accounts.ToListAsync();

            var uri = new Uri(_navManager.Uri);

            _currentYear = _defaultYear;
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(QUERY_YEAR, out var yearStr))
                _currentYear = Convert.ToInt32(yearStr);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(QUERY_MONTH, out var monthStr))
                _currentMonth = Convert.ToInt32(monthStr);

            await RefreshList();
        }
        private async Task ChangeMonth(int? newMonth)
        {
            if (newMonth == _currentMonth)
                return;

            _currentMonth = newMonth;
            await RefreshList();
        }
        private void YearOverflow(bool isIncrement)
        {
            if (stepperYear == null)
                return;

            if (isIncrement)
                _currentYear = ++stepperYear.Value;
            else
                _currentYear = --stepperYear.Value;
        }
        private async Task ChangeYear(int? newYear)
        {
            var year = newYear ?? _defaultYear;
            if (year == _currentYear)
                return;

            _currentYear = year;
            await RefreshList();
        }
        private async Task RefreshList()
        {
            var now = DateTime.UtcNow;
            var start = new DateTime(_currentYear, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = start.AddYears(1);

            if (_currentMonth.HasValue)
            {
                start = new DateTime(_currentYear, _currentMonth.Value, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                end = start.AddMonths(1);
            }

            _list = await _db.Invoices
                .Where(t => (t.Published >= start && t.Published < end) || !t.Published.HasValue)
                .OrderByDescending(t => t.Published)
                .ToListAsync();
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

            var invoice = new Invoice();
            invoice.SequenceNumber = _create.SequenceNumber;
            invoice.Subject = _create.Subject!;
            invoice.IssuerId = _create.IssuerId;
            invoice.ClientId = _create.ClientId;

            var issuer = _issuers.Single(x => x.Id == _create.IssuerId);
            var client = _clients.Single(x => x.Id == _create.ClientId);

            invoice.Currency = client.ContractCurrency;
            invoice.DisplayCurrency = client.DisplayCurrency;
            invoice.IssuerCurrency = issuer.Currency;
            invoice.Note = client.DefaultInvoiceNote;

            var displayAccount = _accounts.FirstOrDefault(a => a.CurrencyId == invoice.DisplayCurrencyId);
            if (displayAccount != null)
                invoice.Account = displayAccount;
            else
            {
                var contractAccount = _accounts.FirstOrDefault(a => a.CurrencyId == invoice.CurrencyId);
                if (contractAccount != null)
                    invoice.Account = contractAccount;
                else
                {
                    var issuerAccount = _accounts.FirstOrDefault(a => a.CurrencyId == invoice.IssuerCurrencyId);
                    if (issuerAccount != null)
                        invoice.Account = issuerAccount;
                    else
                        invoice.Account = _accounts.First();
                }
            }

            await _rates.CalculateRates(invoice);

            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync();
            _create = null;

            _navigator.NavigateTo(C.Routes.InvoiceFor(invoice.Id));
            return default;
        }
    }
}