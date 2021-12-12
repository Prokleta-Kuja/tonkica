using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;
using tonkica.Localization;
using tonkica.Models;
using tonkica.QueryParams;
using tonkica.Services;
using tonkica.Shared;

namespace tonkica.Pages
{
    public partial class Invoices
    {
        [Inject] private AppDbContext Db { get; set; } = null!;
        [Inject] private CurrencyRatesClient Rates { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        private const string QUERY_YEAR = "year";
        private readonly int _defaultYear = DateTime.UtcNow.Year;
        private int _currentYear;
        private const string QUERY_MONTH = "month";
        private readonly int _defaultMonth = DateTime.UtcNow.Month;
        private int? _currentMonth;
        private QueryStepper? StepperYear { get; set; }
        private List<Issuer> _issuers = new();
        private Dictionary<int, string> _issuersD = new();
        private List<Client> _clients = new();
        private Dictionary<int, string> _clientsD = new();
        private List<Account> _accounts = new();
        private Params _params = new(InvoiceCol.Published, true);
        private List<Invoice> _list = new();
        private InvoiceCreateModel? _create;
        private Dictionary<string, string>? _errors;
        private readonly IInvoices _t = LocalizationFactory.Invoices();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // Load currencies into cache
            await Db.Currencies.ToListAsync();

            _issuers = await Db.Issuers.ToListAsync();
            _issuersD = _issuers.ToDictionary(x => x.Id, x => x.Name);

            _clients = await Db.Clients.ToListAsync();
            _clientsD = _clients.ToDictionary(x => x.Id, x => x.Name);

            _accounts = await Db.Accounts.ToListAsync();

            var uri = new Uri(NavManager.Uri);

            _currentYear = _defaultYear;
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(QUERY_YEAR, out var yearStr))
                _currentYear = Convert.ToInt32(yearStr);

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(QUERY_MONTH, out var monthStr))
                _currentMonth = Convert.ToInt32(monthStr);

            await RefreshList();
        }
        private async Task Search(string? term)
        {
            if (string.IsNullOrWhiteSpace(term))
                _params.ClearSearchTerm();
            else
                _params.SetSearchTerm(term);

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
            if (StepperYear == null)
                return;

            if (isIncrement)
                _currentYear = ++StepperYear.Value;
            else
                _currentYear = --StepperYear.Value;
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

            var query = Db.Invoices
                .AsQueryable()
                .Where(i => (i.Published >= start && i.Published < end) || !i.Published.HasValue);

            if (!string.IsNullOrWhiteSpace(_params.SearchTerm))
            {
                var term = _params.SearchTerm.ToUpperInvariant();
                query = query.Where(i =>
                    i.Issuer!.NameNormalized.Contains(term) ||
                    i.Client!.NameNormalized.Contains(term) ||
                    i.Subject.Contains(term)
                );
            }

            switch (_params.OrderBy)
            {
                case InvoiceCol.Issuer:
                    Expression<Func<Invoice, string>> issuer = i => i.Issuer!.Name;
                    query = _params.OrderDesc ? query.OrderByDescending(issuer) : query.OrderBy(issuer);
                    break;
                case InvoiceCol.Client:
                    Expression<Func<Invoice, string>> client = i => i.Client!.Name;
                    query = _params.OrderDesc ? query.OrderByDescending(client) : query.OrderBy(client);
                    break;
                case InvoiceCol.Subject:
                    Expression<Func<Invoice, string>> subject = i => i.Subject;
                    query = _params.OrderDesc ? query.OrderByDescending(subject) : query.OrderBy(subject);
                    break;
                case InvoiceCol.Total:
                    Expression<Func<Invoice, decimal>> total = i => i.Total;
                    query = _params.OrderDesc ? query.OrderByDescending(total) : query.OrderBy(total);
                    break;
                case InvoiceCol.Published:
                    Expression<Func<Invoice, DateTimeOffset?>> published = i => i.Published;
                    query = _params.OrderDesc ? query.OrderByDescending(published) : query.OrderBy(published);
                    break;
                case InvoiceCol.Status:
                    Expression<Func<Invoice, Enums.InvoiceStatus>> status = i => i.Status;
                    query = _params.OrderDesc ? query.OrderByDescending(status) : query.OrderBy(status);
                    break;
                default: break;
            }

            _list = await query.Skip(_params.Skip).ToListAsync();
            StateHasChanged();
        }
        private async Task AddClicked()
        {
            if (!_issuersD.Any())
                _issuersD = await Db.Issuers.ToDictionaryAsync(i => i.Id, i => i.Name);
            if (!_clientsD.Any())
                _clientsD = await Db.Clients.ToDictionaryAsync(i => i.Id, i => i.Name);

            _create = new InvoiceCreateModel();

            _create.IssuerId = _issuersD.FirstOrDefault().Key;
            _create.ClientId = _clientsD.FirstOrDefault().Key;
        }
        private void CancelClicked() { _create = null; _errors = null; }
        private async Task<EventCallback<EventArgs>> SaveClicked()
        {
            if (_create == null)
                return default;

            _errors = _create!.Validate(_t);
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

            var displayAccount = _accounts.FirstOrDefault(a => a.CurrencyId == invoice.DisplayCurrency?.Id);
            if (displayAccount != null)
                invoice.Account = displayAccount;
            else
            {
                var contractAccount = _accounts.FirstOrDefault(a => a.CurrencyId == invoice.Currency?.Id);
                if (contractAccount != null)
                    invoice.Account = contractAccount;
                else
                {
                    var issuerAccount = _accounts.FirstOrDefault(a => a.CurrencyId == invoice.IssuerCurrency?.Id);
                    if (issuerAccount != null)
                        invoice.Account = issuerAccount;
                    else
                        invoice.Account = _accounts.First();
                }
            }

            await Rates.CalculateRates(invoice);

            Db.Invoices.Add(invoice);
            await Db.SaveChangesAsync();
            _create = null;

            NavManager.NavigateTo(C.Routes.InvoiceFor(invoice.Id));
            return default;
        }
    }
}