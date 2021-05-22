using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;
using tonkica.Localization;

namespace tonkica.PrintPages
{
    public partial class InvoiceId
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        [Parameter] public int Id { get; set; }
        private Invoice? _invoice;
        private IPrint? _t;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _invoice = await _db.Invoices
                .Include(x => x.Currency)
                .Include(x => x.IssuerCurrency)
                .Include(x => x.DisplayCurrency)
                .Include(x => x.Items)
                .Include(x => x.Client)
                .Include(x => x.Issuer)
                .Include(x => x.Account)
                .SingleOrDefaultAsync(x => x.Id == Id);

            if (_invoice != null && _invoice.Client != null)
                _t = LocalizationFactory.Print(_invoice.Client.Locale);
        }
    }
}