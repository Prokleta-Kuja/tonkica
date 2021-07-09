namespace tonkica.Localization
{
    public interface ITransactions : IStandard
    {
        string PageTitle { get; }
        string AddTitle { get; }
        string EditTitle { get; }
        string Info { get; }
        string EditCategories { get; }
        string CategoriesTitle { get; }
        string CategoryName { get; }
        string TransactionAccount { get; }
        string TransactiontCategory { get; }
        string TransactionAmount { get; }
        string TransactionDate { get; }
        string TransactionNote { get; }
        string TableCategory { get; }
        string TableAccount { get; }
        string TableAmount { get; }
        string TableDate { get; }
        string TableNote { get; }
    }
    public class Transactions_en : Standard_en, ITransactions
    {
        public string PageTitle => "Transactions";
        public string AddTitle => "Add Transaction";
        public string EditTitle => "Edit";
        public string Info => "Select Transaction to edit or add.";
        public string EditCategories => "Edit Categories";
        public string CategoriesTitle => "Transaction Categories";
        public string CategoryName => "Name";
        public string TransactionAccount => "Account";
        public string TransactiontCategory => "Category";
        public string TransactionAmount => "Amount";
        public string TransactionDate => "Date";
        public string TransactionNote => "Note";
        public string TableCategory => "Category";
        public string TableAccount => "Account";
        public string TableAmount => "Amount";
        public string TableDate => "Date";
        public string TableNote => "Note";
    }
    public class Transactions_hr : Standard_hr, ITransactions
    {
        public string PageTitle => "Transakcije";
        public string AddTitle => "Dodaj transakciju";
        public string EditTitle => "Izmijeni transakciju";
        public string Info => "Odaberi transakciju za izmjenu ili dodaj novu.";
        public string EditCategories => "Izmijeni kategorije";
        public string CategoriesTitle => "Kategorije transakcija";
        public string CategoryName => "Naziv";
        public string TransactionAccount => "Bankovni račun";
        public string TransactiontCategory => "Kategorija";
        public string TransactionAmount => "Iznos";
        public string TransactionDate => "Datum";
        public string TransactionNote => "Opis";
        public string TableCategory => "Kategorija";
        public string TableAccount => "Račun";
        public string TableAmount => "Iznos";
        public string TableDate => "Datum";
        public string TableNote => "Opis";
    }
}