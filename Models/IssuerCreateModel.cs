using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using tonkica.Localization;

namespace tonkica.Models
{
    public class IssuerCreateModel
    {
        public string? Name { get; set; }
        public string? ContactInfo { get; set; }
        public decimal? Limit { get; set; }
        public string? ClockifyUrl { get; set; }
        public int CurrencyId { get; set; }
        public string? IssuedByEmployee { get; set; }
        public string? TimeZone { get; set; }
        public string? Locale { get; set; }
        public string? LogoFileName { get; set; }

        public Dictionary<string, string>? Validate(IIssuers translation)
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add(nameof(Name), translation.ValidationRequired);

            if (string.IsNullOrWhiteSpace(ContactInfo))
                errors.Add(nameof(ContactInfo), translation.ValidationRequired);

            if (CurrencyId <= 0)
                errors.Add(nameof(CurrencyId), translation.ValidationRequired);

            if (Limit.HasValue && Limit.Value < 0)
                errors.Add(nameof(Limit), translation.ValidationLowerThan0);

            if (string.IsNullOrWhiteSpace(IssuedByEmployee))
                errors.Add(nameof(IssuedByEmployee), translation.ValidationRequired);

            if (!string.IsNullOrWhiteSpace(TimeZone))
                try { TimeZoneInfo.FindSystemTimeZoneById(TimeZone); }
                catch (Exception) { errors.Add(nameof(TimeZone), translation.ValidationInvalid); }

            if (!string.IsNullOrWhiteSpace(Locale))
                try { CultureInfo.GetCultureInfo(Locale); }
                catch (Exception) { errors.Add(nameof(Locale), translation.ValidationInvalid); }

            if (errors.Any())
                return errors;

            return null;
        }
    }
}