using System.Collections.Generic;
using tonkica.Localization;

namespace tonkica.Data
{
    public class Issuer
    {
        public Issuer()
        {
            Name = string.Empty;
            ContactInfo = string.Empty;
            IssuedByEmployee = string.Empty;
        }
        public Issuer(string name, string contactInfo, int currencyId, string issueEmployee)
        {
            Name = name;
            ContactInfo = contactInfo;
            CurrencyId = currencyId;
            IssuedByEmployee = issueEmployee;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public decimal? Limit { get; set; }
        public string? ClockifyUrl { get; set; }
        public int CurrencyId { get; set; }
        public string TimeZone { get; set; } = "Europe/Zagreb";
        public string Locale { get; set; } = "hr-HR";
        public string IssuedByEmployee { get; set; }

        public Currency? Currency { get; set; }
        public ICollection<Account>? Accounts { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }

        private Formats? _formats;
        public Formats Formats
        {
            get
            {
                if (_formats == null)
                    _formats = new Formats(Locale, TimeZone);

                return _formats;
            }
        }
    }
}