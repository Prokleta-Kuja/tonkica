using System.Collections.Generic;
using System.Linq;

namespace tonkica.Models
{
    public class ClientCreateModel
    {
        public string? Name { get; set; }
        public string? ContactInfo { get; set; }
        public int ContractCurrencyId { get; set; }
        public int DisplayCurrencyId { get; set; }
        public string? DefaultInvoiceNote { get; set; }

        public Dictionary<string, string>? Validate()
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add(nameof(Name), "Required");

            if (string.IsNullOrWhiteSpace(ContactInfo))
                errors.Add(nameof(ContactInfo), "Required");

            if (ContractCurrencyId <= 0)
                errors.Add(nameof(ContractCurrencyId), "Required");

            if (DisplayCurrencyId <= 0)
                errors.Add(nameof(DisplayCurrencyId), "Required");

            if (errors.Any())
                return errors;

            return null;
        }
    }
}