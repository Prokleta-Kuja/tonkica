using System.Collections.Generic;

namespace tonkica.Data
{
    public class Issuer
    {
        public Issuer()
        {
            Name = string.Empty;
            ContactInfo = string.Empty;
        }
        public Issuer(string name, string contactInfo, int currencyId)
        {
            Name = name;
            ContactInfo = contactInfo;
            CurrencyId = currencyId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public decimal? Limit { get; set; }
        public string? ClockifyUrl { get; set; }
        public int CurrencyId { get; set; }

        public Currency? Currency { get; set; }
        public ICollection<Account>? Accounts { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
    }
}