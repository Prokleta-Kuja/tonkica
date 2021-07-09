using System;

namespace tonkica.Localization
{
    public static class LocalizationFactory
    {
        const string ENV_LOCALE = "LOCALE";
        const string ENV_TZ = "TZ";
        const string DEFAULT_LOCALE = "hr";
        const string DEFAULT_TZ = "America/Chicago";
        static string EnvLocale => Environment.GetEnvironmentVariable(ENV_LOCALE) ?? DEFAULT_LOCALE;
        static string EnvTZ => Environment.GetEnvironmentVariable(ENV_TZ) ?? DEFAULT_TZ;

        public static Formats Formats() => Formats(EnvLocale, EnvTZ);
        public static Formats Formats(string locale, string timeZone) => new Formats(locale, timeZone);
        public static IIndex Index() => Index(EnvLocale);
        public static IIndex Index(string locale)
        {
            if (locale.StartsWith("hr"))
                return new Index_hr();

            return new Index_en();
        }
        public static ITransactions Transactions() => Transactions(EnvLocale);
        public static ITransactions Transactions(string locale)
        {
            if (locale.StartsWith("hr"))
                return new Transactions_hr();

            return new Transactions_en();
        }
        public static IClients Clients() => Clients(EnvLocale);
        public static IClients Clients(string locale)
        {
            if (locale.StartsWith("hr"))
                return new Clients_hr();

            return new Clients_en();
        }
        public static IAccounts Accounts() => Accounts(EnvLocale);
        public static IAccounts Accounts(string locale)
        {
            if (locale.StartsWith("hr"))
                return new Accounts_hr();

            return new Accounts_en();
        }
        public static IPrint Print() => Print(EnvLocale);
        public static IPrint Print(string locale)
        {
            if (locale.StartsWith("hr"))
                return new Print_hr();

            return new Print_en();
        }
    }
}