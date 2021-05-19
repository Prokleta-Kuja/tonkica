using tonkica.Localization.Print;

namespace tonkica.Localization
{
    public static class LocalizationFactory
    {
        public static Formats Formats(string locale, string timeZone) => new Formats(locale, timeZone);
        public static IPrint Print(string locale)
        {
            if (locale.StartsWith("hr"))
                return new Print_hr();

            return new Print_en();
        }
    }
}