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
    public partial class Accounts
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        private IList<Currency> _currencies = new List<Currency>();
        private Dictionary<int, string> _currenciesD = new Dictionary<int, string>();
        private IList<Issuer> _issuers = new List<Issuer>();
        private Dictionary<int, string> _issuersD = new Dictionary<int, string>();
        private IList<Account> _list = new List<Account>();
        private Account _item = new Account();
        private AccountCreateModel? _create;
        private AccountEditModel? _edit;
        private Dictionary<string, string>? _errors;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currencies = await _db.Currencies.ToListAsync();
            _currenciesD = _currencies.ToDictionary(x => x.Id, x => x.Tag);
            _issuers = await _db.Issuers.ToListAsync();
            _issuersD = _issuers.ToDictionary(x => x.Id, x => x.Name);
            _list = await _db.Accounts.ToListAsync();
        }
        private void AddClicked()
        {
            _edit = null;
            _create = new AccountCreateModel();
            _create.CurrencyId = _currenciesD.FirstOrDefault().Key;
        }
        private void EditClicked(Account item)
        {
            _create = null;
            _edit = new AccountEditModel(item);
        }
        private void CancelClicked() { _create = null; _edit = null; }
        private async Task<EventCallback<EventArgs>> SaveCreateClicked()
        {
            if (_create == null)
                return default;

            _errors = _create.Validate();
            if (_errors != null)
                return default;

            var Account = new Account();
            Account.Name = _create.Name!;
            Account.Info = _create.Info!;
            Account.CurrencyId = _create.CurrencyId;
            Account.IssuerId = _create.IssuerId;

            _db.Accounts.Add(Account);
            await _db.SaveChangesAsync();

            _list.Insert(0, Account);
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

            var Account = _list.SingleOrDefault(x => x.Id == _edit.Id);
            if (Account == null)
                return default;

            Account.Name = _edit.Name!;
            Account.Info = _edit.Info!;
            Account.CurrencyId = _edit.CurrencyId;
            Account.IssuerId = _edit.IssuerId;

            await _db.SaveChangesAsync();
            _edit = null;

            return default;
        }
    }
}