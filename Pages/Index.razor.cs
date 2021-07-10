using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;
using tonkica.Localization;
using tonkica.Models;

namespace tonkica.Pages
{
    public partial class Index
    {
        [Inject] private AppDbContext Db { get; set; } = null!;
        [Inject] private NavigationManager NavManager { get; set; } = null!;
        private const string QUERY_YEAR = "year";
        private readonly int _defaultYear = DateTime.UtcNow.Year;
        private int _currentYear;
        private IDictionary<int, Issuer> _issuers = new Dictionary<int, Issuer>();
        private IDictionary<int, DashboardModel> _issuerDashboards = new Dictionary<int, DashboardModel>();
        private IIndex _t = LocalizationFactory.Index();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _issuers = await Db.Issuers
                .Include(i => i.Currency)
                .ToDictionaryAsync(i => i.Id);

            var year = _defaultYear;
            var uri = new Uri(NavManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(QUERY_YEAR, out var yearStr))
                year = Convert.ToInt32(yearStr);

            await ChangeYear(year);
        }
        private async Task ChangeYear(int? newYear)
        {
            var year = newYear ?? _defaultYear;
            if (year == _currentYear)
                return;

            _currentYear = year;
            _issuerDashboards = new Dictionary<int, DashboardModel>();
            var start = new DateTime(year, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var end = start.AddYears(1).AddSeconds(-1);

            var transactions = await Db.Transactions
                .Include(t => t.Account)
                .Include(t => t.Category)
                .Where(t => t.Date > start && t.Date < end)
                .ToListAsync();

            var invoices = await Db.Invoices
                .Where(t => t.Published > start && t.Published < end || !t.Published.HasValue)
                .ToListAsync();

            foreach (var transaction in transactions)
            {
                int issuerId = transaction.Account!.IssuerId;
                if (!_issuerDashboards.ContainsKey(issuerId))
                    _issuerDashboards.Add(issuerId, new DashboardModel());

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
            }

            foreach (var invoice in invoices)
            {
                var issuerId = invoice.IssuerId;
                if (!_issuerDashboards.ContainsKey(issuerId))
                    _issuerDashboards.Add(issuerId, new DashboardModel());

                var dash = _issuerDashboards[issuerId];
                dash.Issued += invoice.IssuerTotal;
            }

            foreach (var issuer in _issuerDashboards)
                issuer.Value.PostProcess();

            StateHasChanged();
        }
    }
}