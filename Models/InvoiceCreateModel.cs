using System.Collections.Generic;
using System.Linq;

namespace tonkica.Models
{
    public class InvoiceCreateModel
    {
        public string? Subject { get; set; }
        public int IssuerId { get; set; }
        public int ClientId { get; set; }
        public int CurrencyId { get; set; }
        public int DisplayCurrencyId { get; set; }
        public int IssuerCurrencyId { get; set; }
        public decimal DispalyRate { get; set; }
        public decimal IssuerRate { get; set; }
        public string? Note { get; set; }

        public Dictionary<string, string>? Validate()
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Subject))
                errors.Add(nameof(Subject), "Required");

            if (IssuerId <= 0)
                errors.Add(nameof(IssuerId), "Required");

            if (ClientId <= 0)
                errors.Add(nameof(ClientId), "Required");

            if (CurrencyId <= 0)
                errors.Add(nameof(CurrencyId), "Required");

            if (DisplayCurrencyId <= 0)
                errors.Add(nameof(DisplayCurrencyId), "Required");

            if (IssuerCurrencyId <= 0)
                errors.Add(nameof(IssuerCurrencyId), "Required");

            if (DispalyRate <= 0)
                errors.Add(nameof(DispalyRate), "Required");

            if (IssuerRate <= 0)
                errors.Add(nameof(IssuerRate), "Required");

            if (errors.Any())
                return errors;

            return null;
        }
    }
}