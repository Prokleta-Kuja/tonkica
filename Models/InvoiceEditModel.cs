using System;
using System.Collections.Generic;
using System.Linq;
using tonkica.Data;
using tonkica.Enums;
using tonkica.Localization;

namespace tonkica.Models
{
    public class InvoiceEditModel
    {
        public string? Subject { get; set; }
        public string? SequenceNumber { get; set; }
        public int IssuerId { get; set; }
        public int ClientId { get; set; }
        public int CurrencyId { get; set; }
        public int AccountId { get; set; }
        public int DisplayCurrencyId { get; set; }
        public int IssuerCurrencyId { get; set; }
        public DateTimeOffset? Published { get; set; }
        public int Status { get; set; }
        public string? Note { get; set; }
        readonly Invoice _original;

        public InvoiceEditModel(Invoice i)
        {
            _original = i;
            SequenceNumber = i.SequenceNumber;
            Subject = i.Subject;
            IssuerId = i.IssuerId;
            ClientId = i.ClientId;
            CurrencyId = i.CurrencyId;
            AccountId = i.AccountId;
            DisplayCurrencyId = i.DisplayCurrencyId;
            IssuerCurrencyId = i.IssuerCurrencyId;
            Published = i.Published;
            Status = (int)i.Status;
            Note = i.Note;
        }
        public bool IsDirty =>
            SequenceNumber != _original.SequenceNumber ||
            Subject != _original.Subject ||
            IssuerId != _original.IssuerId ||
            ClientId != _original.ClientId ||
            CurrencyId != _original.CurrencyId ||
            AccountId != _original.AccountId ||
            DisplayCurrencyId != _original.DisplayCurrencyId ||
            IssuerCurrencyId != _original.IssuerCurrencyId ||
            Published != _original.Published ||
            Status != (int)_original.Status ||
            Note != _original.Note;


        public Dictionary<string, string>? Validate(IInvoice translation)
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

            if (AccountId <= 0)
                errors.Add(nameof(AccountId), translation.ValidationRequired);

            if (Published.HasValue && Published.Value > DateTimeOffset.UtcNow)
                errors.Add(nameof(Published), translation.ValidationCannotBeInTheFuture);

            if (errors.Any())
                return errors;

            return null;
        }
    }
}