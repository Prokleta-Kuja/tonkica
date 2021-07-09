namespace tonkica.Localization
{
    public interface IClients : IStandard
    {
        string PageTitle { get; }
        string AddTitle { get; }
        string EditTitle { get; }
        string Info { get; }
        string ClientName { get; }
        string ClientContactInfo { get; }
        string ClientNote { get; }
        string ClientDisplay { get; }
        string ClientRate { get; }
        string ClientContract { get; }
        string ClientDueDate { get; }
        string ClientTimeZone { get; }
        string ClientLocale { get; }
        string TableName { get; }
        string TableContactInfo { get; }
        string TableContract { get; }
        string TableRate { get; }
        string TableNote { get; }
    }
    public class Clients_en : Standard_en, IClients
    {
        public string PageTitle => "Clients";
        public string AddTitle => "Add Client";
        public string EditTitle => "Edit";
        public string Info => "Select Client to edit or add.";
        public string ClientName => "Name";
        public string ClientContactInfo => "Contact Info";
        public string ClientNote => "Default invoice note";
        public string ClientDisplay => "Display Currency";
        public string ClientRate => "Contract Rate";
        public string ClientContract => "Contract Currency";
        public string ClientDueDate => "Due in days";
        public string ClientTimeZone => "Time Zone";
        public string ClientLocale => "Locale";
        public string TableName => "Name";
        public string TableContactInfo => "Contact Info";
        public string TableContract => "Contract";
        public string TableRate => "Rate";
        public string TableNote => "Default Note";
    }
    public class Clients_hr : Standard_hr, IClients
    {
        public string PageTitle => "Klijenti";
        public string AddTitle => "Dodaj klijenta";
        public string EditTitle => "Izmijeni klijenta";
        public string Info => "Odaberi klijenta za izmjenu ili dodaj novog.";
        public string ClientName => "Naziv";
        public string ClientContactInfo => "Kontakt podaci";
        public string ClientNote => "Zadana napomena na računu";
        public string ClientDisplay => "Valuta za prikaz";
        public string ClientRate => "Ugovorena satnica";
        public string ClientContract => "Ugovorena valuta";
        public string ClientDueDate => "Dospijeće";
        public string ClientTimeZone => "Vremenska zona";
        public string ClientLocale => "Lokalitet";
        public string TableName => "Naziv";
        public string TableContactInfo => "Kontakt";
        public string TableContract => "Valuta";
        public string TableRate => "Satnica";
        public string TableNote => "Zadana napomena";
    }
}