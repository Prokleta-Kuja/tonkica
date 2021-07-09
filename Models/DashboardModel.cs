using System;
using System.Collections.Generic;

namespace tonkica.Models
{
    public class DashboardModel
    {
        const decimal LEVEL_1 = 85_000;
        const decimal LEVEL_2 = 115_000;
        const decimal LEVEL_3 = 149_500;
        const decimal LEVEL_4 = 230_000;
        const decimal LEVEL_5 = 300_000;
        public decimal Income { get; set; }
        public decimal IncomePercentage { get; set; }
        public decimal Expense { get; set; }
        public decimal ExpensePercentage { get; set; }
        public Dictionary<string, decimal> IncomeCategories { get; set; } = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> ExpenseCategories { get; set; } = new Dictionary<string, decimal>();
        public decimal Issued { get; set; }
        public int CurrentLevel { get; set; }
        public decimal TillNextLevel { get; set; }

        public void PostProcess()
        {
            CalculatePercentages();
            CalculateLevel();
        }
        private void CalculatePercentages()
        {
            var expenseAbs = Math.Abs(Expense);
            var total = Income + expenseAbs;
            if (total == 0)
                return;

            IncomePercentage = (Income / total) * 100;
            ExpensePercentage = (expenseAbs / total) * 100;
        }
        private void CalculateLevel()
        {
            if (Income < LEVEL_1)
            {
                CurrentLevel = 1;
                TillNextLevel = LEVEL_1 - Income;
            }
            else if (Income < LEVEL_2)
            {
                CurrentLevel = 2;
                TillNextLevel = LEVEL_2 - Income;
            }
            else if (Income < LEVEL_3)
            {
                CurrentLevel = 3;
                TillNextLevel = LEVEL_3 - Income;
            }
            else if (Income < LEVEL_4)
            {
                CurrentLevel = 4;
                TillNextLevel = LEVEL_4 - Income;
            }
            else if (Income < LEVEL_5)
            {
                CurrentLevel = 5;
                TillNextLevel = LEVEL_5 - Income;
            }
        }
    }
}