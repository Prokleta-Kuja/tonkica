namespace tonkica.Localization
{
    public interface IAccounts
    {
        string PageTitle { get; }
        string AddTitle { get; }
        string Add { get; }
        string EditTitle { get; }
        string Edit { get; }
        string Submit { get; }
        string Cancel { get; }
        string Info { get; }

        string AccountName { get; }
        string AccountInfo { get; }
        string AccountCurrency { get; }
    }
    public class Accounts_en : IAccounts
    {
        public string PageTitle => "Accounts";
        public string AddTitle => "Add Account";
        public string Add => "Add";
        public string EditTitle => "Edit";
        public string Edit => "Edit";
        public string Submit => "Submit";
        public string Cancel => "Cancel";
        public string Info => "Select Account to edit or add.";
        public string AccountName => "Name";
        public string AccountInfo => "Info";
        public string AccountCurrency => "Currency";
    }
    public class Accounts_hr : IAccounts
    {
        public string PageTitle => "Bankovni računi";
        public string AddTitle => "Dodaj bankovni račun";
        public string Add => "Dodaj";
        public string EditTitle => "Izmijeni bankovni račun";
        public string Edit => "Izmijeni";
        public string Submit => "Spremi";
        public string Cancel => "Odustani";
        public string Info => "Odaberi bankovni račun za izmjenu ili dodaj novi.";
        public string AccountName => "Naziv";
        public string AccountInfo => "Podaci";
        public string AccountCurrency => "Valuta";
    }
}