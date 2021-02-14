using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;

namespace tonkica.PrintPages
{
    public partial class InvoiceId
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        [Parameter] public int Id { get; set; }
        private Invoice? _invoice;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _invoice = await _db.Invoices
                .Include(x => x.Items)
                .Include(x => x.Client)
                .Include(x => x.Issuer)
                .Include(x => x.Account)
                .SingleOrDefaultAsync(x => x.Id == Id);
        }
    }
}