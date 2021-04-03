using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using tonkica.Data;

namespace tonkica.Pages
{
    public partial class Index
    {
        [Inject] private AppDbContext _db { get; set; } = null!;
        private IDictionary<int, Issuer> _issuers = new Dictionary<int, Issuer>();
        private IDictionary<int, (decimal Income, decimal Expense)> _transactionSums = new Dictionary<int, (decimal Income, decimal Expense)>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _issuers = await _db.Issuers.ToDictionaryAsync(i => i.Id);
            await ChangeYear(DateTime.UtcNow.Year);
        }
        private async Task ChangeYear(int year)
        {
            var start = new DateTime(year, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var end = start.AddYears(1).AddSeconds(-1);

            var transactions = await _db.Transactions
                .Include(t => t.Account)
                .Where(t => t.Date > start && t.Date < end)
                .ToListAsync();

            foreach (var transaction in transactions)
            {
                int issuerId = transaction.Account!.IssuerId;
                if (!_transactionSums.ContainsKey(issuerId))
                    _transactionSums.Add(transaction.Account!.IssuerId, (0, 0));

                var sum = _transactionSums[issuerId];
                if (transaction.IssuerAmount > 0)
                    sum.Income += transaction.IssuerAmount;
                else
                    sum.Expense += transaction.IssuerAmount;

                _transactionSums[issuerId] = sum;
            }
        }
    }
}