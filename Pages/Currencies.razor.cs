using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;

namespace tonkica.Pages
{
    public partial class Currencies
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        private IList<Currency> _currencies = new List<Currency>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _currencies = await _db.Currencies.ToListAsync();
        }
    }
}