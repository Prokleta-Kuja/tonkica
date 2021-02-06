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
    public partial class Invoices
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        private IList<Currency> _currencies = new List<Currency>();
        private Dictionary<int, string> _issuersD = new Dictionary<int, string>();
        private Dictionary<int, string> _clientsD = new Dictionary<int, string>();
        private IList<Invoice> _list = new List<Invoice>();
        private InvoiceCreateModel? _item;
        private Dictionary<string, string>? _errors;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currencies = await _db.Currencies.ToListAsync();
            _list = await _db.Invoices
                .Include(i => i.Client)
                .ToListAsync();
        }
        private async Task AddClicked()
        {
            if (!_issuersD.Any())
                _issuersD = await _db.Issuers.ToDictionaryAsync(i => i.Id, i => i.Name);
            if (!_clientsD.Any())
                _clientsD = await _db.Clients.ToDictionaryAsync(i => i.Id, i => i.Name);

            _item = new InvoiceCreateModel();

            _item.CurrencyId = _currencies.First().Id;
            _item.IssuerId = _issuersD.FirstOrDefault().Key;
            _item.ClientId = _clientsD.FirstOrDefault().Key;
        }
        private void CancelClicked() { _item = null; _errors = null; }
        private async Task<EventCallback<EventArgs>> SaveClicked()
        {
            _errors = _item!.Validate();

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