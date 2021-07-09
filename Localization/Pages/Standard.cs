namespace tonkica.Localization
{
    public interface IStandard
    {
        string Search { get; }
        string Add { get; }
        string Edit { get; }
        string Remove { get; }
        string Close { get; }
        string SaveChanges { get; }
        string Save { get; }
        string Cancel { get; }
    }
    public class Standard_en : IStandard
    {
        public string Search => "Search";
        public string Add => "Add";
        public string Edit => "Edit";
        public string Remove => "Remove";
        public string Close => "Close";
        public string SaveChanges => "Save changes";
        public string Save => "Submit";
        public string Cancel => "Cancel";
    }
    public class Standard_hr : IStandard
    {
        public string Search => "Traži";
        public string Add => "Dodaj";
        public string Edit => "Izmijeni";
        public string Remove => "Ukloni";
        public string Close => "Zatvori";
        public string SaveChanges => "Spremi izmjene";
        public string Save => "Spremi";
        public string Cancel => "Odustani";
    }
}