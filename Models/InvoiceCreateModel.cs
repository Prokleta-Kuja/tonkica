using System.Collections.Generic;
using System.Linq;
using tonkica.Localization;

namespace tonkica.Models
{
    public class InvoiceCreateModel
    {
        public string? Subject { get; set; }
        public string? SequenceNumber { get; set; }
        public int IssuerId { get; set; }
        public int ClientId { get; set; }

        public Dictionary<string, string>? Validate(IInvoices translation)
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Subject))
                errors.Add(nameof(Subject), translation.ValidationRequired);

            if (string.IsNullOrWhiteSpace(SequenceNumber))
                errors.Add(nameof(SequenceNumber), translation.ValidationRequired);

            if (IssuerId <= 0)
                errors.Add(nameof(IssuerId), translation.ValidationRequired);

            if (ClientId <= 0)
                errors.Add(nameof(ClientId), translation.ValidationRequired);

            if (errors.Any())
                return errors;

            return null;
        }
    }
}