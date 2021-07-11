using System;

namespace tonkica.Localization
{
    public static class LocalizationFactory
    {
        const string ENV_LOCALE = "LOCALE";
        const string ENV_TZ = "TZ";
        const string DEFAULT_LOCALE = "hr";
        const string DEFAULT_TZ = "Europe/Zagreb";
        static string EnvLocale => Environment.GetEnvironmentVariable(ENV_LOCALE) ?? DEFAULT_LOCALE;
        static string EnvTZ => Environment.GetEnvironmentVariable(ENV_TZ) ?? DEFAULT_TZ;

        public static Formats Formats() => Formats(EnvLocale, EnvTZ);
        public static Formats Formats(string locale, string timeZone) => new(locale, timeZone);
        public static IStandard Standard() => Standard(EnvLocale);
        public static IStandard Standard(string locale)
        {
            if (locale.StartsWith("hr"))
                return new Standard_hr();

            return new Standard_en();
        }
        public static INavigation Navigation() => Navigation(EnvLocale);
        public static INavigation Navigation(string locale)
        {
            if (locale.StartsWith("hr"))
                return new Navigation_hr();

            return new Navigation_en();
        }
        public static IIndex Index() => Index(EnvLocale);
        public static IIndex Index(string locale)
        {
            if (locale.StartsWith("hr"))
                return new Index_hr();

            return new Index_en();
        }
        public static IInvoices Invoices() => Invoices(EnvLocale);
        public static IInvoices Invoices(string locale)
        {
            if (locale.StartsWith("hr"))
                return new Invoices_hr();

            return new Invoices_en();
        }
        public static IInvoice Invoice() => Invoice(EnvLocale);
        public static IInvoice Invoice(string locale)
        {
            if (locale.StartsWith("hr"))
                return new Invoice_hr();

            return new Invoice_en();
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
        public static IIssuers Issuers() => Issuers(EnvLocale);
        public static IIssuers Issuers(string locale)
        {
            if (locale.StartsWith("hr"))
                return new Issuers_hr();

            return new Issuers_en();
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