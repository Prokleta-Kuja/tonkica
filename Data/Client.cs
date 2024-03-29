using System.Collections.Generic;
using tonkica.Localization;

namespace tonkica.Data
{
    public class Client
    {
        public Client()
        {
            Name = string.Empty;
            ContactInfo = string.Empty;
        }
        public Client(string name, string contactInfo, int contractCurrencyId, int displayCurrencyId)
        {
            Name = name;
            ContactInfo = contactInfo;
            ContractCurrencyId = contractCurrencyId;
            DisplayCurrencyId = displayCurrencyId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NameNormalized { get => Name.ToUpperInvariant(); private set { } }
        public string ContactInfo { get; set; }
        public int ContractCurrencyId { get; set; }
        public decimal ContractRate { get; set; }
        public int DisplayCurrencyId { get; set; }
        public decimal DueInDays { get; set; }
        public string? DefaultInvoiceNote { get; set; }
        public string TimeZone { get; set; } = "America/Chicago";
        public string Locale { get; set; } = "en-US";

        public Currency? ContractCurrency { get; set; }
        public Currency? DisplayCurrency { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = null!;

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