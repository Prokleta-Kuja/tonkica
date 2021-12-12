using System;
using System.Collections.Generic;
using System.Linq;
using tonkica.Localization;

namespace tonkica.Models
{
    public class TransactionCreateModel
    {
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public decimal? Amount { get; set; }
        public DateTimeOffset? Date { get; set; }
        public string? Note { get; set; }

        public Dictionary<string, string>? Validate(ITransactions translation)
        {
            var errors = new Dictionary<string, string>();

            if (AccountId <= 0)
                errors.Add(nameof(AccountId), translation.ValidationRequired);

            if (CategoryId <= 0)
                errors.Add(nameof(CategoryId), translation.ValidationRequired);

            if (!Amount.HasValue)
                errors.Add(nameof(Amount), translation.ValidationRequired);

            if (!Date.HasValue)
                errors.Add(nameof(Date), translation.ValidationRequired);

            if (Date.HasValue && Date.Value > DateTimeOffset.Now)
                errors.Add(nameof(Date), translation.ValidationNotInFuture);

            if (errors.Any())
                return errors;

            return null;
        }
    }
}