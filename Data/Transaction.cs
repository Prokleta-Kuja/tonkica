using System;

namespace tonkica.Data
{
    public class Transaction
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? Note { get; set; }

        public Account? Account { get; set; }
    }
}