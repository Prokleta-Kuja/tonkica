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
        public Account(string name, string info, int currencyId, int issuerId)
        {
            Name = name;
            Info = info;
            CurrencyId = currencyId;
            IssuerId = issuerId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NameNormalized { get => Name.ToUpperInvariant(); private set { } }
        public string Info { get; set; }
        public int CurrencyId { get; set; }
        public int IssuerId { get; set; }

        public Currency? Currency { get; set; }
        public Issuer? Issuer { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = null!;
    }
}