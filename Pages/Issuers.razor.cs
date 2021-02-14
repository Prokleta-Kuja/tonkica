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
    public partial class Issuers
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        private IList<Currency> _currencies = new List<Currency>();
        private Dictionary<int, string> _currenciesD = new Dictionary<int, string>();
        private IList<Issuer> _list = new List<Issuer>();
        private Issuer _item = new Issuer();
        private IssuerCreateModel? _create;
        private IssuerEditModel? _edit;
        private Dictionary<string, string>? _errors;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currencies = await _db.Currencies.ToListAsync();
            _currenciesD = _currencies.ToDictionary(x => x.Id, x => x.Tag);
            _list = await _db.Issuers.ToListAsync();
        }
        private void AddClicked()
        {
            _edit = null;
            _create = new IssuerCreateModel();
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

            _errors = _create.Validate();
            if (_errors != null)
                return default;

            var issuer = new Issuer();
            issuer.Name = _create.Name!;
            issuer.ContactInfo = _create.ContactInfo!;
            issuer.CurrencyId = _create.CurrencyId;
            issuer.ClockifyUrl = _create.ClockifyUrl;

            _db.Issuers.Add(issuer);
            await _db.SaveChangesAsync();

            _list.Insert(0, issuer);
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

            var issuer = _list.SingleOrDefault(x => x.Id == _edit.Id);
            if (issuer == null)
                return default;

            issuer.Name = _edit.Name!;
            issuer.ContactInfo = _edit.ContactInfo!;
            issuer.CurrencyId = _edit.CurrencyId;
            issuer.ClockifyUrl = _edit.ClockifyUrl;

            await _db.SaveChangesAsync();
            _edit = null;

            return default;
        }
    }
}