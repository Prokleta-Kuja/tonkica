using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;
using tonkica.Localization;
using tonkica.Models;

namespace tonkica.Pages
{
    public partial class Accounts
    {
        [Inject] private AppDbContext Db { get; set; } = null!;
        private List<Currency> _currencies = new();
        private Dictionary<int, string> _currenciesD = new();
        private List<Issuer> _issuers = new();
        private Dictionary<int, string> _issuersD = new();
        private List<Account> _list = new();
        private AccountCreateModel? _create;
        private AccountEditModel? _edit;
        private Dictionary<string, string>? _errors;
        private readonly IAccounts _t = LocalizationFactory.Accounts();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currencies = await Db.Currencies.ToListAsync();
            _currenciesD = _currencies.ToDictionary(x => x.Id, x => x.Tag);
            _issuers = await Db.Issuers.ToListAsync();
            _issuersD = _issuers.ToDictionary(x => x.Id, x => x.Name);
            _list = await Db.Accounts.ToListAsync();
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

            Db.Accounts.Add(Account);
            await Db.SaveChangesAsync();

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

            await Db.SaveChangesAsync();
            _edit = null;

            return default;
        }
    }
}