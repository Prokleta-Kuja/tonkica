using System;
using System.Collections.Generic;
using System.Linq;
using tonkica.Data;
using tonkica.Enums;

namespace tonkica.Models
{
    public class InvoiceEditModel
    {
        public string? Subject { get; set; }
        public string? SequenceNumber { get; set; }
        public int IssuerId { get; set; }
        public int ClientId { get; set; }
        public int CurrencyId { get; set; }
        public int DisplayCurrencyId { get; set; }
        public int IssuerCurrencyId { get; set; }
        public DateTimeOffset? Published { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }

        public InvoiceEditModel(Invoice i)
        {
            SequenceNumber = i.SequenceNumber;
            Subject = i.Subject;
            IssuerId = i.IssuerId;
            ClientId = i.ClientId;
            CurrencyId = i.CurrencyId;
            DisplayCurrencyId = i.DisplayCurrencyId;
            IssuerCurrencyId = i.IssuerCurrencyId;
            Published = i.Published;
            Status = (int)i.Status;
            Note = i.Note;
        }

        public Dictionary<string, string>? Validate()
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Subject))
                errors.Add(nameof(Subject), "Required");

            if (IssuerId <= 0)
                errors.Add(nameof(IssuerId), "Required");

            if (ClientId <= 0)
                errors.Add(nameof(ClientId), "Required");

            if (Published.HasValue && Published.Value > DateTimeOffset.UtcNow)
                errors.Add(nameof(Published), "Cannot be in the future");

            if (errors.Any())
                return errors;

            return null;
        }
    }
}