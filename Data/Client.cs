using System.Collections.Generic;

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
        public string ContactInfo { get; set; }
        public int ContractCurrencyId { get; set; }
        public int DisplayCurrencyId { get; set; }
        public string? DefaultInvoiceNote { get; set; }

        public Currency? ContractCurrency { get; set; }
        public Currency? DisplayCurrency { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
    }
}