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
    public partial class Clients
    {
        [Inject] private AppDbContext Db { get; set; } = null!;
        private List<Currency> _currencies = new();
        private Dictionary<int, string> _currenciesD = new();
        private List<Client> _list = new();
        private ClientCreateModel? _create;
        private ClientEditModel? _edit;
        private Dictionary<string, string>? _errors;
        private readonly IClients _t = LocalizationFactory.Clients();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currencies = await Db.Currencies.ToListAsync();
            _currenciesD = _currencies.ToDictionary(x => x.Id, x => x.Tag);
            _list = await Db.Clients.ToListAsync();
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

            _errors = _create.Validate(_t);
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

            Db.Clients.Add(client);
            await Db.SaveChangesAsync();

            _list.Insert(0, client);
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

            await Db.SaveChangesAsync();
            _edit = null;

            return default;
        }
    }
}