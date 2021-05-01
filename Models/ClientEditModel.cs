using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using tonkica.Data;

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

        public Dictionary<string, string>? Validate()
        {
            var errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(Name))
                errors.Add(nameof(Name), "Required");

            if (string.IsNullOrWhiteSpace(ContactInfo))
                errors.Add(nameof(ContactInfo), "Required");

            if (ContractCurrencyId <= 0)
                errors.Add(nameof(ContractCurrencyId), "Required");

            if (!ContractRate.HasValue)
                errors.Add(nameof(ContractRate), "Required");

            if (DisplayCurrencyId <= 0)
                errors.Add(nameof(DisplayCurrencyId), "Required");

            if (DueInDays <= 0)
                errors.Add(nameof(DueInDays), "Must be larger then 0");

            if (!string.IsNullOrWhiteSpace(TimeZone))
                try { TimeZoneInfo.FindSystemTimeZoneById(TimeZone); }
                catch (Exception) { errors.Add(nameof(TimeZone), "Invalid"); }

            if (!string.IsNullOrWhiteSpace(Locale))
                try { CultureInfo.GetCultureInfo(Locale); }
                catch (Exception) { errors.Add(nameof(Locale), "Invalid"); }

            if (errors.Any())
                return errors;

            return null;
        }
    }
}