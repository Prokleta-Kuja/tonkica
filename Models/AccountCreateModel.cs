using System.Collections.Generic;
using System.Linq;
using tonkica.Localization;

namespace tonkica.Models
{
    public class AccountCreateModel
    {
        public string? Name { get; set; }
        public string? Info { get; set; }
        public int CurrencyId { get; set; }
        public int IssuerId { get; set; }

        public Dictionary<string, string>? Validate(IAccounts translation)
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add(nameof(Name), translation.ValidationRequired);

            if (string.IsNullOrWhiteSpace(Info))
                errors.Add(nameof(Info), translation.ValidationRequired);

            if (CurrencyId <= 0)
                errors.Add(nameof(CurrencyId), translation.ValidationRequired);

            if (IssuerId <= 0)
                errors.Add(nameof(IssuerId), translation.ValidationRequired);

            if (errors.Any())
                return errors;

            return null;
        }
    }
}