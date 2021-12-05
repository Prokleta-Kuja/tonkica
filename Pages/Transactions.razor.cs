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
    public partial class Transactions
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
        private List<Account> _accounts = new();
        private Dictionary<int, string> _accountsD = new();
        private List<TransactionCategory> Categories { get; set; } = null!;
        private Dictionary<int, string> _categoriesD = new();
        private Params _params = new(TransactionCol.Date, true);
        private List<Transaction> _list = new();
        private TransactionCreateModel? _create;
        private TransactionEditModel? _edit;
        private string? _newCategory;
        private Dictionary<string, string>? _errors;
        private readonly ITransactions _t = LocalizationFactory.Transactions();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _accounts = await Db.Accounts
                .Include(a => a.Currency)
                .Include(a => a.Issuer)
                .ToListAsync();
            _accountsD = _accounts.ToDictionary(x => x.Id, x => x.Name);

            Categories = await Db.TransactionCategories.ToListAsync();
            _categoriesD = Categories.ToDictionary(x => x.Id, x => x.Name);

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

            var query = Db.Transactions
                .AsQueryable()
                .Where(t => t.Date >= start && t.Date < end);

            if (!string.IsNullOrWhiteSpace(_params.SearchTerm))
            {
                var term = _params.SearchTerm.ToUpperInvariant();
                query = query.Where(t =>
                    t.Category!.NameNormalized.Contains(term) ||
                    t.Account!.NameNormalized.Contains(term) ||
                    t.NoteNormalized!.Contains(term)
                );
            }

            switch (_params.OrderBy)
            {
                case TransactionCol.Category:
                    Expression<Func<Transaction, string>> categoryName = t => t.Category!.Name;
                    query = _params.OrderDesc ? query.OrderByDescending(categoryName) : query.OrderBy(categoryName);
                    break;
                case TransactionCol.Account:
                    Expression<Func<Transaction, string>> account = t => t.Account!.Name;
                    query = _params.OrderDesc ? query.OrderByDescending(account) : query.OrderBy(account);
                    break;
                case TransactionCol.Amount:
                    Expression<Func<Transaction, decimal>> amount = t => t.Amount;
                    query = _params.OrderDesc ? query.OrderByDescending(amount) : query.OrderBy(amount);
                    break;
                case TransactionCol.Date:
                    Expression<Func<Transaction, DateTimeOffset>> date = t => t.Date;
                    query = _params.OrderDesc ? query.OrderByDescending(date) : query.OrderBy(date);
                    break;
                case TransactionCol.Note:
                    Expression<Func<Transaction, string?>> note = t => t.Note;
                    query = _params.OrderDesc ? query.OrderByDescending(note) : query.OrderBy(note);
                    break;
                default: break;
            }

            _list = await query.Skip(_params.Skip).ToListAsync();
            StateHasChanged();
        }
        private void AddClicked()
        {
            _edit = null;
            _create = new TransactionCreateModel { Date = DateTimeOffset.Now };
            _create.AccountId = _accountsD.FirstOrDefault().Key;
            _create.CategoryId = _categoriesD.FirstOrDefault().Key;
        }
        private void EditClicked(Transaction item)
        {
            _create = null;
            _edit = new TransactionEditModel(item);
        }
        private void CancelClicked() { _create = null; _edit = null; }
        private async Task<EventCallback<EventArgs>> SaveCreateClicked()
        {
            if (_create == null)
                return default;

            _errors = _create.Validate();
            if (_errors != null)
                return default;

            var account = _accounts.Single(a => a.Id == _create.AccountId);
            var category = Categories.Single(c => c.Id == _create.CategoryId);

            var transaction = new Transaction();
            transaction.AccountId = account.Id;
            transaction.Account = account;
            transaction.CategoryId = category.Id;
            transaction.Category = category;
            transaction.IssuerCurrencyId = account.Issuer!.CurrencyId;
            transaction.Amount = _create.Amount!.Value;
            transaction.Date = _create.Date!.Value;
            transaction.Note = _create.Note;

            await Rates.CalculateRates(transaction);
            transaction.IssuerAmount = transaction.Amount * transaction.IssuerRate;

            Db.Transactions.Add(transaction);
            await Db.SaveChangesAsync();

            _list.Insert(0, transaction);
            _create = null;

            return default;
        }
        private async Task<EventCallback<EventArgs>> SaveEditClicked()
        {
            if (_edit == null)
                return default;

            _errors = _edit.Validate();
            if (_errors != null)
                return default;

            var transaction = _list.SingleOrDefault(x => x.Id == _edit.Id);
            if (transaction == null)
                return default;

            var account = _accounts.Single(a => a.Id == _edit.AccountId);
            var category = Categories.Single(c => c.Id == _edit.CategoryId);

            transaction.AccountId = account.Id;
            transaction.Account = account;
            transaction.CategoryId = category.Id;
            transaction.Category = category;
            transaction.IssuerCurrencyId = account.Issuer!.CurrencyId;
            transaction.Amount = _edit.Amount!.Value;
            transaction.Date = _edit.Date!.Value;
            transaction.Note = _edit.Note;

            await Rates.CalculateRates(transaction);
            transaction.IssuerAmount = transaction.Amount * transaction.IssuerRate;

            await Db.SaveChangesAsync();
            _edit = null;

            return default;
        }
        private async Task SaveCategories()
        {
            await Db.SaveChangesAsync();
        }
        private async Task RemoveCategory(TransactionCategory category)
        {
            Db.TransactionCategories.Remove(category);
            await Db.SaveChangesAsync();
            Categories = Categories.Where(i => i.Id != category.Id).ToList();
            _categoriesD.Remove(category.Id);
        }
        private async Task AddCategory()
        {
            if (string.IsNullOrWhiteSpace(_newCategory))
                return;

            var category = new TransactionCategory { Name = _newCategory };
            Db.TransactionCategories.Add(category);
            await Db.SaveChangesAsync();

            Categories.Add(category);
            _categoriesD.Add(category.Id, category.Name);

            _newCategory = string.Empty;
        }
    }
}