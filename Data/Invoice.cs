using System;
using System.Collections.Generic;

namespace tonkica.Data
{
    public class Invoice
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public DateTimeOffset? Published { get; set; }
        public int CurrencyId { get; set; }
        public int DisplayCurrencyId { get; set; }
        public int IssuerCurrencyId { get; set; }
        public decimal Rate { get; set; } = 1;
        public decimal Total { get; set; }
        public decimal DisplayTotal { get; set; }
        public decimal IssuerTotal { get; set; }

        public Client? Client { get; set; }
        public Currency? Currency { get; set; }
        public Currency? DisplayCurrency { get; set; }
        public Currency? IssuerCurrency { get; set; }
        public ICollection<InvoiceItem>? Items { get; set; }
    }
}