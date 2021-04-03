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
    public partial class Transactions
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        [Inject] private CurrencyRatesClient _rates { get; set; } = null!;
        private IList<Account> _accounts = new List<Account>();
        private Dictionary<int, string> _accountsD = new Dictionary<int, string>();
        private IList<TransactionCategory> _categories { get; set; } = null!;
        private Dictionary<int, string> _categoriesD = new Dictionary<int, string>();
        private IList<Transaction> _list = new List<Transaction>();
        private Transaction _item = new Transaction();
        private TransactionCreateModel? _create;
        private TransactionEditModel? _edit;
        private string? _newCategory;
        private Dictionary<string, string>? _errors;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _accounts = await _db.Accounts
                .Include(a => a.Currency)
                .Include(a => a.Issuer)
                .ToListAsync();
            _accountsD = _accounts.ToDictionary(x => x.Id, x => x.Name);

            _categories = await _db.TransactionCategories.ToListAsync();
            _categoriesD = _categories.ToDictionary(x => x.Id, x => x.Name);
            _list = await _db.Transactions.OrderByDescending(t => t.Date).ToListAsync();
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
            var category = _categories.Single(c => c.Id == _create.CategoryId);

            var transaction = new Transaction();
            transaction.AccountId = account.Id;
            transaction.Account = account;
            transaction.CategoryId = category.Id;
            transaction.Category = category;
            transaction.IssuerCurrencyId = account.Issuer!.CurrencyId;
            transaction.Amount = _create.Amount!.Value;
            transaction.Date = _create.Date!.Value;
            transaction.Note = _create.Note;

            await _rates.CalculateRates(transaction);
            transaction.IssuerAmount = transaction.Amount * transaction.IssuerRate;

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();

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
            var category = _categories.Single(c => c.Id == _edit.CategoryId);

            transaction.AccountId = account.Id;
            transaction.Account = account;
            transaction.CategoryId = category.Id;
            transaction.Category = category;
            transaction.IssuerCurrencyId = account.Issuer!.CurrencyId;
            transaction.Amount = _edit.Amount!.Value;
            transaction.Date = _edit.Date!.Value;
            transaction.Note = _edit.Note;

            await _rates.CalculateRates(transaction);
            transaction.IssuerAmount = transaction.Amount * transaction.IssuerRate;

            await _db.SaveChangesAsync();
            _edit = null;

            return default;
        }
        private async Task SaveCategories()
        {
            await _db.SaveChangesAsync();
        }
        private async Task RemoveCategory(TransactionCategory category)
        {
            _db.TransactionCategories.Remove(category);
            await _db.SaveChangesAsync();
            _categories = _categories.Where(i => i.Id != category.Id).ToList();
            _categoriesD.Remove(category.Id);
        }
        private async Task AddCategory()
        {
            if (string.IsNullOrWhiteSpace(_newCategory))
                return;

            var category = new TransactionCategory { Name = _newCategory };
            _db.TransactionCategories.Add(category);
            await _db.SaveChangesAsync();

            _categories.Add(category);
            _categoriesD.Add(category.Id, category.Name);

            _newCategory = string.Empty;
        }
    }
}