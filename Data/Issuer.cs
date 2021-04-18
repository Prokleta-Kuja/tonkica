using System;
using System.Collections.Generic;
using System.Globalization;

namespace tonkica.Data
{
    public class Issuer
    {
        private const string DEFAULT_TZ = "Europe/Zagreb";
        private const string DEFAULT_LOCALE = "hr-HR";
        TimeZoneInfo? _tz;
        CultureInfo? _ci;
        public Issuer()
        {
            Name = string.Empty;
            ContactInfo = string.Empty;
        }
        public Issuer(string name, string contactInfo, int currencyId)
        {
            Name = name;
            ContactInfo = contactInfo;
            CurrencyId = currencyId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public decimal? Limit { get; set; }
        public string? ClockifyUrl { get; set; }
        public int CurrencyId { get; set; }
        public string? TimeZone { get; set; }
        public string? Locale { get; set; }

        public Currency? Currency { get; set; }
        public ICollection<Account>? Accounts { get; set; }
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