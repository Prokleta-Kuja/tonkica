using System;
using System.Globalization;

namespace tonkica.Localization
{
    public class Formats
    {
        const string DEFAULT_TZ = "Europe/Zagreb";
        const string DEFAULT_LOCALE = "hr-HR";
        public TimeZoneInfo TimeZone { get; private set; }
        public CultureInfo Culture { get; private set; }
        public Formats(string locale, string timeZone)
        {
            if (TimeZone == null || TimeZone.Id != timeZone)
                try
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone ?? DEFAULT_TZ);
                }
                catch (Exception)
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById(DEFAULT_TZ);
                }

            if (Culture == null || Culture.Name != locale)
                try
                {
                    Culture = CultureInfo.GetCultureInfo(locale ?? DEFAULT_LOCALE);
                }
                catch (Exception)
                {
                    Culture = CultureInfo.GetCultureInfo(DEFAULT_LOCALE);
                }
        }
        public string Display(DateTimeOffset? dt, bool showTime = true, bool showOffset = false, string empty = "-")
        {
            if (!dt.HasValue)
                return empty;

            var printDt = TimeZoneInfo.ConvertTimeFromUtc(dt.Value.UtcDateTime, TimeZone!);
            var format = Culture!.DateTimeFormat.ShortDatePattern;

            if (showTime)
                format += $" {Culture.DateTimeFormat.ShortTimePattern}";

            if (showOffset)
            {
                var offset = TimeZone!.GetUtcOffset(printDt);
                var sign = offset.TotalMilliseconds < 0 ? '-' : '+';
                format += $" {sign}{offset:h\\:mm}";
            }

            return printDt.ToString(format);
        }
        public string Display(decimal? num, int places = 2, string empty = "-")
        {
            if (!num.HasValue)
                return empty;

            return num.Value.ToString($"#,##0.{new string('0', places)}", Culture!.NumberFormat);
        }
    }
}