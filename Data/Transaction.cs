using System;

namespace tonkica.Data
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int CategoryId { get; set; } = 1; // Uncategorized
        public decimal Amount { get; set; }
        public int IssuerCurrencyId { get; set; }
        public decimal IssuerRate { get; set; }
        public decimal IssuerAmount { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? Note { get; set; }

        public TransactionCategory? Category { get; set; }
        public Account? Account { get; set; }
        public Currency? IssuerCurrency { get; set; }
    }
}