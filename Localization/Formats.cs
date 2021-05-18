using System;
using System.Globalization;

namespace tonkica.Localization
{
    public class Formats
    {
        const string DEFAULT_TZ = "Europe/Zagreb";
        const string DEFAULT_LOCALE = "hr-HR";
        TimeZoneInfo _tz;
        CultureInfo _ci;
        public Formats(string locale, string timeZone)
        {
            if (_tz == null || _tz.Id != timeZone)
                try
                {
                    _tz = TimeZoneInfo.FindSystemTimeZoneById(timeZone ?? DEFAULT_TZ);
                }
                catch (Exception)
                {
                    _tz = TimeZoneInfo.FindSystemTimeZoneById(DEFAULT_TZ);
                }

            if (_ci == null || _ci.Name != locale)
                try
                {
                    _ci = CultureInfo.GetCultureInfo(locale ?? DEFAULT_LOCALE);
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

            return num.Value.ToString($"#,##0.{new string('0', places)}", _ci!.NumberFormat);
        }
    }
}