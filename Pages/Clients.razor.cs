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
    public partial class Clients
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        private IList<Currency> _currencies = new List<Currency>();
        private Dictionary<int, string> _currenciesD = new Dictionary<int, string>();
        private IList<Client> _list = new List<Client>();
        private ClientCreateModel? _create;
        private ClientEditModel? _edit;
        private Dictionary<string, string>? _errors;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currencies = await _db.Currencies.ToListAsync();
            _currenciesD = _currencies.ToDictionary(x => x.Id, x => x.Tag);
            _list = await _db.Clients.ToListAsync();
        }
        private void AddClicked()
        {
            _edit = null;
            _create = new();
            _create.DisplayCurrencyId = _currenciesD.FirstOrDefault().Key;
            _create.ContractCurrencyId = _currenciesD.FirstOrDefault().Key;
        }
        private void EditClicked(Client item)
        {
            _create = null;
            _edit = new ClientEditModel(item);
        }
        private void CancelClicked() { _create = null; _edit = null; }
        private async Task<EventCallback<EventArgs>> SaveCreateClicked()
        {
            if (_create == null)
                return default;

            _errors = _create.Validate();
            if (_errors != null)
                return default;

            var client = new Client();
            client.Name = _create.Name!;
            client.ContactInfo = _create.ContactInfo!;
            client.ContractCurrencyId = _create.ContractCurrencyId;
            client.ContractRate = _create.ContractRate ?? 0;
            client.DisplayCurrencyId = _create.DisplayCurrencyId;
            client.DueInDays = _create.DueInDays ?? 30;
            client.DefaultInvoiceNote = _create.DefaultInvoiceNote;
            client.TimeZone = _create.TimeZone!;
            client.Locale = _create.Locale!;

            _db.Clients.Add(client);
            await _db.SaveChangesAsync();

            _list.Insert(0, client);
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

            var client = _list.SingleOrDefault(x => x.Id == _edit.Id);
            if (client == null)
                return default;

            client.Name = _edit.Name!;
            client.ContactInfo = _edit.ContactInfo!;
            client.ContractCurrencyId = _edit.ContractCurrencyId;
            client.ContractRate = _edit.ContractRate ?? 0;
            client.DisplayCurrencyId = _edit.DisplayCurrencyId;
            client.DueInDays = _edit.DueInDays ?? 30;
            client.DefaultInvoiceNote = _edit.DefaultInvoiceNote;
            client.TimeZone = _edit.TimeZone!;
            client.Locale = _edit.Locale!;

            await _db.SaveChangesAsync();
            _edit = null;

            return default;
        }
    }
}