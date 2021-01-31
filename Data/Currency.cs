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

        public ICollection<Client>? ContractClients { get; set; }
        public ICollection<Client>? DisplayClients { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
        public ICollection<Invoice>? DisplayInvoices { get; set; }
        public ICollection<Invoice>? IssuerInvoices { get; set; }
    }
}