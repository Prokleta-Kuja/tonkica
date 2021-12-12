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
    public partial class Issuers
    {
        [Inject] private AppDbContext Db { get; set; } = null!;
        private List<Currency> _currencies = new();
        private Dictionary<int, string> _currenciesD = new();
        private List<Issuer> _list = new();
        private IssuerCreateModel? _create;
        private IssuerEditModel? _edit;
        private Dictionary<string, string>? _errors;
        private readonly IIssuers _t = LocalizationFactory.Issuers();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currencies = await Db.Currencies.ToListAsync();
            _currenciesD = _currencies.ToDictionary(x => x.Id, x => x.Tag);
            _list = await Db.Issuers.ToListAsync();
        }
        private void AddClicked()
        {
            _edit = null;
            _create = new();
            _create.CurrencyId = _currenciesD.FirstOrDefault().Key;
        }
        private void EditClicked(Issuer item)
        {
            _create = null;
            _edit = new IssuerEditModel(item);
        }
        private void CancelClicked() { _create = null; _edit = null; }
        private async Task<EventCallback<EventArgs>> SaveCreateClicked()
        {
            if (_create == null)
                return default;

            _errors = _create.Validate(_t);
            if (_errors != null)
                return default;

            var issuer = new Issuer();
            issuer.Name = _create.Name!;
            issuer.IssuedByEmployee = _create.IssuedByEmployee!;
            issuer.ContactInfo = _create.ContactInfo!;
            issuer.CurrencyId = _create.CurrencyId;
            issuer.Limit = _create.Limit;
            issuer.ClockifyUrl = _create.ClockifyUrl;
            issuer.TimeZone = _create.TimeZone!;
            issuer.Locale = _create.Locale!;

            Db.Issuers.Add(issuer);
            await Db.SaveChangesAsync();

            _list.Insert(0, issuer);
            _create = null;

            return default;
        }
        private async Task<EventCallback<EventArgs>> SaveEditClicked()
        {
            if (_edit == null)
                return default;

            _errors = _edit.Validate(_t);
            if (_errors != null)
                return default;

            var issuer = _list.SingleOrDefault(x => x.Id == _edit.Id);
            if (issuer == null)
                return default;

            issuer.Name = _edit.Name!;
            issuer.IssuedByEmployee = _edit.IssuedByEmployee!;
            issuer.ContactInfo = _edit.ContactInfo!;
            issuer.CurrencyId = _edit.CurrencyId;
            issuer.Limit = _edit.Limit;
            issuer.ClockifyUrl = _edit.ClockifyUrl;
            issuer.TimeZone = _edit.TimeZone!;
            issuer.Locale = _edit.Locale!;

            await Db.SaveChangesAsync();
            _edit = null;

            return default;
        }
    }
}