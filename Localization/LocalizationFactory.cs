using tonkica.Localization.Print;

namespace tonkica.Localization
{
    public static class LocalizationFactory
    {
        public static Formats Formats(string locale, string timeZone) => new Formats(locale, timeZone);
        public static IPrint Print(string locale)
        {
            switch (locale)
            {
                case "hr-HR":
                case "hr":
                    return new Print_hr();

                default:
                    return new Print_en();
            }
        }
    }
}