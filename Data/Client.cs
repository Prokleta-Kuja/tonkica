using System;
using System.Collections.Generic;
using System.Globalization;

namespace tonkica.Data
{
    public class Client
    {
        private const string DEFAULT_TZ = "America/Chicago";
        private const string DEFAULT_LOCALE = "en-US";
        TimeZoneInfo? _tz;
        CultureInfo? _ci;
        public Client()
        {
            Name = string.Empty;
            ContactInfo = string.Empty;
        }
        public Client(string name, string contactInfo, int contractCurrencyId, int displayCurrencyId)
        {
            Name = name;
            ContactInfo = contactInfo;
            ContractCurrencyId = contractCurrencyId;
            DisplayCurrencyId = displayCurrencyId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public int ContractCurrencyId { get; set; }
        public decimal ContractRate { get; set; }
        public int DisplayCurrencyId { get; set; }
        public string? DefaultInvoiceNote { get; set; }
        public string? TimeZone { get; set; }
        public string? Locale { get; set; }

        public Currency? ContractCurrency { get; set; }
        public Currency? DisplayCurrency { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }

        public TimeZoneInfo TimeZoneInfo
        {
            get
            {
                InitializeLocale();
                return _tz!;
            }
        }
        public CultureInfo CultureInfo
        {
            get
            {
                InitializeLocale();
                return _ci!;
            }
        }

        private void InitializeLocale()
        {
            if (_tz == null || _tz.Id != TimeZone)
                try
                {
                    _tz = TimeZoneInfo.FindSystemTimeZoneById(TimeZone ?? DEFAULT_TZ);
                }
                catch (Exception)
                {
                    _tz = TimeZoneInfo.FindSystemTimeZoneById(DEFAULT_TZ);
                }

            if (_ci == null || _ci.Name != Locale)
                try
                {
                    _ci = CultureInfo.GetCultureInfo(Locale ?? DEFAULT_LOCALE);
                }
                catch (Exception)
                {
                    _ci = CultureInfo.GetCultureInfo(DEFAULT_LOCALE);
                }
        }
        public string Display(DateTimeOffset? dt, bool showTime = true, bool showOffset = false, string empty = "-")
        {
            if (!dt.HasValue)
                return empty;

            InitializeLocale();

            var printDt = TimeZoneInfo.ConvertTimeFromUtc(dt.Value.UtcDateTime, _tz!);
            var format = _ci!.DateTimeFormat.ShortDatePattern;

            if (showTime)
                format += $" {_ci.DateTimeFormat.ShortTimePattern}";

            if (showOffset)
            {
                var offset = _tz!.GetUtcOffset(printDt);
                var sign = offset.TotalMilliseconds < 0 ? '-' : '+';
                format += $" {sign}{offset:h\\:mm}";
            }

            return printDt.ToString(format);
        }
        public string Display(decimal? num, int places = 2, string empty = "-")
        {
            if (!num.HasValue)
                return empty;

            InitializeLocale();

            return num.Value.ToString($"#,##0.{new string('0', places)}", _ci!.NumberFormat);
        }
    }
}