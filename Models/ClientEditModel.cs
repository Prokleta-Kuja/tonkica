using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using tonkica.Data;
using tonkica.Localization;

namespace tonkica.Models
{
    public class ClientEditModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ContactInfo { get; set; }
        public int ContractCurrencyId { get; set; }
        public decimal? ContractRate { get; set; }
        public int DisplayCurrencyId { get; set; }
        public decimal? DueInDays { get; set; }
        public string? DefaultInvoiceNote { get; set; }
        public string? TimeZone { get; set; }
        public string? Locale { get; set; }

        public ClientEditModel(Client c)
        {
            Id = c.Id;
            Name = c.Name;
            ContactInfo = c.ContactInfo;
            ContractCurrencyId = c.ContractCurrencyId;
            ContractRate = c.ContractRate;
            DisplayCurrencyId = c.DisplayCurrencyId;
            DueInDays = c.DueInDays;
            DefaultInvoiceNote = c.DefaultInvoiceNote;
            TimeZone = c.TimeZone;
            Locale = c.Locale;
        }

        public Dictionary<string, string>? Validate(IClients translation)
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add(nameof(Name), translation.ValidationRequired);

            if (string.IsNullOrWhiteSpace(ContactInfo))
                errors.Add(nameof(ContactInfo), translation.ValidationRequired);

            if (ContractCurrencyId <= 0)
                errors.Add(nameof(ContractCurrencyId), translation.ValidationRequired);

            if (!ContractRate.HasValue)
                errors.Add(nameof(ContractRate), translation.ValidationRequired);

            if (DisplayCurrencyId <= 0)
                errors.Add(nameof(DisplayCurrencyId), translation.ValidationRequired);

            if (DueInDays <= 0)
                errors.Add(nameof(DueInDays), translation.ValidationLargerThan0);

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