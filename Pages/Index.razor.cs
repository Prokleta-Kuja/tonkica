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
    public partial class Index
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        private IDictionary<int, Issuer> _issuers = new Dictionary<int, Issuer>();
        private IDictionary<int, DashboardModel> _issuerDashboards = new Dictionary<int, DashboardModel>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _issuers = await _db.Issuers
                .Include(i => i.Currency)
                .ToDictionaryAsync(i => i.Id);
            await ChangeYear(DateTime.UtcNow.Year);
        }
        private async Task ChangeYear(int year)
        {
            var start = new DateTime(year, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var end = start.AddYears(1).AddSeconds(-1);

            var transactions = await _db.Transactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .Where(t => t.Date > start && t.Date < end)
                .ToListAsync();

            foreach (var transaction in transactions)
            {
                int issuerId = transaction.Account!.IssuerId;
                if (!_issuerDashboards.ContainsKey(issuerId))
                    _issuerDashboards.Add(transaction.Account!.IssuerId, new DashboardModel());

                var dash = _issuerDashboards[issuerId];
                var category = transaction.Category!.Name;

                if (transaction.IssuerAmount > 0)
                {
                    dash.Income += transaction.IssuerAmount;
                    if (!dash.IncomeCategories.ContainsKey(category))
                        dash.IncomeCategories.Add(category, transaction.IssuerAmount);
                    else
                        dash.IncomeCategories[category] += transaction.IssuerAmount;
                }
                else
                {
                    dash.Expense += transaction.IssuerAmount;
                    if (!dash.ExpenseCategories.ContainsKey(category))
                        dash.ExpenseCategories.Add(category, transaction.IssuerAmount);
                    else
                        dash.ExpenseCategories[category] += transaction.IssuerAmount;
                }

                var quarter = (transaction.Date.Month + 2) / 3;
                if (!dash.Quarters.ContainsKey(quarter))
                    dash.Quarters.Add(quarter, transaction.IssuerAmount);
                else
                    dash.Quarters[quarter] += transaction.IssuerAmount;
            }
        }
    }
}