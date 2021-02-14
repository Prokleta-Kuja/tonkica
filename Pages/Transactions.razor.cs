using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;
using tonkica.Models;

namespace tonkica.Pages
{
    public partial class Transactions
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        private IList<Account> _accounts = new List<Account>();
        private Dictionary<int, string> _accountsD = new Dictionary<int, string>();
        private IList<Transaction> _list = new List<Transaction>();
        private Transaction _item = new Transaction();
        private TransactionCreateModel? _create;
        private TransactionEditModel? _edit;
        private Dictionary<string, string>? _errors;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _accounts = await _db.Accounts.ToListAsync();
            _accountsD = _accounts.ToDictionary(x => x.Id, x => x.Name);
            _list = await _db.Transactions.ToListAsync();
        }
        private void AddClicked()
        {
            _edit = null;
            _create = new TransactionCreateModel();
            _create.AccountId = _accountsD.FirstOrDefault().Key;
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

            var transaction = new Transaction();
            transaction.AccountId = _create.AccountId;
            transaction.Amount = _create.Amount!.Value;
            transaction.Date = _create.Date!.Value;
            transaction.Note = _create.Note;


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

            transaction.AccountId = _edit.AccountId;
            transaction.Amount = _edit.Amount!.Value;
            transaction.Date = _edit.Date!.Value;
            transaction.Note = _edit.Note;

            await _db.SaveChangesAsync();
            _edit = null;

            return default;
        }
    }
}