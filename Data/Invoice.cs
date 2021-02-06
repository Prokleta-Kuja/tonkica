using System;
using System.Collections.Generic;
using tonkica.Enums;

namespace tonkica.Data
{
    public class Invoice
    {
        public int Id { get; set; }
        public string? SequenceNumber { get; set; }
        public string Subject { get; set; } = null!;
        public int IssuerId { get; set; }
        public int ClientId { get; set; }
        public int CurrencyId { get; set; }
        public int DisplayCurrencyId { get; set; }
        public int IssuerCurrencyId { get; set; }
        public decimal DisplayRate { get; set; }
        public decimal IssuerRate { get; set; }
        public decimal Total { get; set; }
        public decimal DisplayTotal { get; set; }
        public decimal IssuerTotal { get; set; }
        public DateTimeOffset? Published { get; set; }
        public InvoiceStatus Status { get; set; }
        public string? Note { get; set; }

        public Issuer? Issuer { get; set; }
        public Client? Client { get; set; }
        public Currency? Currency { get; set; }
        public Currency? DisplayCurrency { get; set; }
        public Currency? IssuerCurrency { get; set; }
        public ICollection<InvoiceItem>? Items { get; set; }
    }
}