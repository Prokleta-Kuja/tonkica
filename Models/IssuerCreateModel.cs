using System.Collections.Generic;
using System.Linq;

namespace tonkica.Models
{
    public class IssuerCreateModel
    {
        public string? Name { get; set; }
        public string? ContactInfo { get; set; }
        public int CurrencyId { get; set; }

        public Dictionary<string, string>? Validate()
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add(nameof(Name), "Required");

            if (string.IsNullOrWhiteSpace(ContactInfo))
                errors.Add(nameof(ContactInfo), "Required");

            if (CurrencyId <= 0)
                errors.Add(nameof(CurrencyId), "Required");

            if (errors.Any())
                return errors;

            return null;
        }
    }
}