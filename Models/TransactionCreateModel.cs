using System;
using System.Collections.Generic;
using System.Linq;

namespace tonkica.Models
{
    public class TransactionCreateModel
    {
        public int AccountId { get; set; }
        public decimal? Amount { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string? Note { get; set; }

        public Dictionary<string, string>? Validate()
        {
            var errors = new Dictionary<string, string>();

            if (AccountId <= 0)
                errors.Add(nameof(AccountId), "Required");

            if (!Amount.HasValue)
                errors.Add(nameof(Amount), "Required");

            if (!Date.HasValue)
                errors.Add(nameof(Date), "Required");

            if (Date.HasValue && Date.Value > DateTimeOffset.Now)
                errors.Add(nameof(Date), "Cannot be in the future");

            if (errors.Any())
                return errors;

            return null;
        }
    }
}