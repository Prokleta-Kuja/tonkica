using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;
using tonkica.Enums;
using tonkica.Models;

namespace tonkica.Pages
{
    public partial class InvoiceId
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        [Parameter] public int Id { get; set; }
        private IList<Currency> _currencies = new List<Currency>();
        private Dictionary<int, string> _currenciesD = new Dictionary<int, string>();
        private Dictionary<int, string> _issuersD = new Dictionary<int, string>();
        private Dictionary<int, string> _clientsD = new Dictionary<int, string>();
        private Dictionary<int, string> _statusesD = new Dictionary<int, string>();
        private IList<Invoice> _list = new List<Invoice>();
        private Invoice? _item;
        private InvoiceEditModel? _edit;
        private Dictionary<string, string>? _errors;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            foreach (var e in Enum.GetValues(typeof(InvoiceStatus)))
                _statusesD.Add((int)e, e.ToString()!);

            _currencies = await _db.Currencies.ToListAsync();
            _currenciesD = _currencies.ToDictionary(x => x.Id, x => x.Tag);

            var issuers = await _db.Issuers.ToListAsync();
            _issuersD = issuers.ToDictionary(x => x.Id, x => x.Name);

            var clients = await _db.Clients.ToListAsync();
            _clientsD = clients.ToDictionary(x => x.Id, x => x.Name);

            _item = await _db.Invoices
                .Include(x => x.Items)
                .SingleOrDefaultAsync(x => x.Id == Id);

            if (_item != null)
                _edit = new InvoiceEditModel(_item);
        }
        private async Task<EventCallback<EventArgs>> SaveClicked()
        {
            _errors = _edit!.Validate();

            await Task.CompletedTask;

            // if (_item == null)
            //     throw new System.ArgumentNullException(nameof(_item));

            // var adding = _item.Id <= 0;
            // if (adding)
            //     _db.Invoices.Add(_item);
            // else
            // {
            //     var existing = _list.Single(i => i.Id == _item.Id);
            //     existing.Subject = _item.Subject;
            //     existing.IssuerId = _item.IssuerId;
            //     existing.ClientId = _item.ClientId;
            //     existing.CurrencyId = _item.CurrencyId;
            //     existing.DisplayCurrencyId = _item.DisplayCurrencyId;
            //     existing.Published = _item.Published;
            //     existing.Status = _item.Status;
            //     existing.Note = _item.Note;
            // }

            // await _db.SaveChangesAsync();

            // if (adding)
            //     _list.Insert(0, _item);

            // _formShown = false;

            return default;
        }
    }
}