namespace tonkica.Localization
{
    public interface INavigation : IStandard
    {
        string App { get; }
        string Invoices { get; }
        string Transactions { get; }
        string Clients { get; }
        string Accounts { get; }
        string Issuers { get; }
    }
    public class Navigation_en : Standard_en, INavigation
    {
        public string App => "tonkica";
        public string Invoices => "Invoices";
        public string Transactions => "Transactions";
        public string Clients => "Clients";
        public string Accounts => "Accounts";
        public string Issuers => "Issuers";
    }
    public class Navigation_hr : Standard_hr, INavigation
    {
        public string App => "tonkica";
        public string Invoices => "Računi";
        public string Transactions => "Transakcije";
        public string Clients => "Klijenti";
        public string Accounts => "Bankovni računi";
        public string Issuers => "Izdavači";
    }
}