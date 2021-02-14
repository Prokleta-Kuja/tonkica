using System;
using System.Collections.Generic;
using System.Linq;
using tonkica.Data;

namespace tonkica.Models
{
    public class TransactionEditModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal? Amount { get; set; }
        public DateTimeOffset Date { get; set; }
        public string? Note { get; set; }

        public TransactionEditModel(Transaction t)
        {
            Id = t.Id;
            AccountId = t.AccountId;
            Amount = t.Amount;
            Date = t.Date;
            Note = t.Note;
        }

        public Dictionary<string, string>? Validate()
        {
            var errors = new Dictionary<string, string>();

            if (AccountId <= 0)
                errors.Add(nameof(AccountId), "Required");

            if (!Amount.HasValue)
                errors.Add(nameof(Amount), "Required");


            if (errors.Any())
                return errors;

            return null;
        }
    }
}