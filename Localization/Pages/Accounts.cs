namespace tonkica.Localization
{
    public interface IAccounts : IStandard
    {
        string PageTitle { get; }
        string AddTitle { get; }
        string EditTitle { get; }
        string Info { get; }
        string AccountName { get; }
        string AccountInfo { get; }
        string AccountIssuer { get; }
        string AccountCurrency { get; }
    }
    public class Accounts_en : Standard_en, IAccounts
    {
        public string PageTitle => "Accounts";
        public string AddTitle => "Add Account";
        public string EditTitle => "Edit";
        public string Info => "Select Account to edit or add.";
        public string AccountName => "Name";
        public string AccountInfo => "Info";
        public string AccountIssuer => "Issuer";
        public string AccountCurrency => "Currency";
    }
    public class Accounts_hr : Standard_hr, IAccounts
    {
        public string PageTitle => "Bankovni računi";
        public string AddTitle => "Dodaj bankovni račun";
        public string EditTitle => "Izmijeni bankovni račun";
        public string Info => "Odaberi bankovni račun za izmjenu ili dodaj novi.";
        public string AccountName => "Naziv";
        public string AccountInfo => "Podaci";
        public string AccountIssuer => "Izdavač";
        public string AccountCurrency => "Valuta";
    }
}