using System.Collections.Generic;

namespace tonkica.Data
{
    public class Account
    {
        public Account()
        {
            Name = string.Empty;
            Info = string.Empty;
        }
        public Account(string name, string info, int currencyId)
        {
            Name = name;
            Info = info;
            CurrencyId = currencyId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public int CurrencyId { get; set; }

        public Currency? Currency { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}