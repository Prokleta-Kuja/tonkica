using System.Collections.Generic;
using System.Linq;

namespace tonkica.Models
{
    public class InvoiceCreateModel
    {
        public string? Subject { get; set; }
        public string? SequenceNumber { get; set; }
        public int IssuerId { get; set; }
        public int ClientId { get; set; }

        public Dictionary<string, string>? Validate()
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Subject))
                errors.Add(nameof(Subject), "Required");

            if (IssuerId <= 0)
                errors.Add(nameof(IssuerId), "Required");

            if (ClientId <= 0)
                errors.Add(nameof(ClientId), "Required");

            if (errors.Any())
                return errors;

            return null;
        }
    }
}