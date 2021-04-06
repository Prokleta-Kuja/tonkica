using System.Collections.Generic;

namespace tonkica.Models
{
    public class DashboardModel
    {
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public Dictionary<string, decimal> IncomeCategories { get; set; } = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> ExpenseCategories { get; set; } = new Dictionary<string, decimal>();
        public Dictionary<int, decimal> Quarters { get; set; } = new Dictionary<int, decimal>();
    }
}