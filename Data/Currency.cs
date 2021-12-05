using System.Collections.Generic;

namespace tonkica.Data
{
    public class Currency
    {
        public Currency(string tag, string symbol, bool isPrefix)
        {
            Tag = tag;
            Symbol = symbol;
            IsPrefix = isPrefix;
        }

        public int Id { get; set; }
        public string Tag { get; set; }
        public string Symbol { get; set; }
        public bool IsPrefix { get; set; }

        public ICollection<Issuer> Issuers { get; set; } = null!;
        public ICollection<Client> ContractClients { get; set; } = null!;
        public ICollection<Client> DisplayClients { get; set; } = null!;
        public ICollection<Invoice> Invoices { get; set; } = null!;
        public ICollection<Invoice> DisplayInvoices { get; set; } = null!;
        public ICollection<Invoice> IssuerInvoices { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = null!;
        public ICollection<Account> Accounts { get; set; } = null!;
    }
}