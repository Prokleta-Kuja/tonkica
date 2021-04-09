using System.Collections.Generic;
using System.Linq;
using tonkica.Data;

namespace tonkica.Models
{
    public class IssuerEditModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContactInfo { get; set; }
        public decimal? Limit { get; set; }
        public string? ClockifyUrl { get; set; }
        public int CurrencyId { get; set; }

        public IssuerEditModel(Issuer i)
        {
            Id = i.Id;
            Name = i.Name;
            ContactInfo = i.ContactInfo;
            CurrencyId = i.CurrencyId;
            Limit = i.Limit;
            ClockifyUrl = i.ClockifyUrl;
        }

        public Dictionary<string, string>? Validate()
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add(nameof(Name), "Required");

            if (string.IsNullOrWhiteSpace(ContactInfo))
                errors.Add(nameof(ContactInfo), "Required");

            if (CurrencyId <= 0)
                errors.Add(nameof(CurrencyId), "Required");

            if (Limit.HasValue && Limit.Value < 0)
                errors.Add(nameof(Limit), "Cannot be lower than 0");

            if (errors.Any())
                return errors;

            return null;
        }
    }
}