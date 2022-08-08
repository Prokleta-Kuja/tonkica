namespace tonkica.Localization
{
    public interface IIssuers : IStandard
    {
        string PageTitle { get; }
        string AddTitle { get; }
        string EditTitle { get; }
        string Info { get; }
        string IssuerName { get; }
        string IssuerEmployee { get; }
        string IssuerEmployeeInfo { get; }
        string IssuerCurrency { get; }
        string IssuerLimit { get; }
        string IssuerContact { get; }
        string IssuerContactInfo { get; }
        string IssuerTimeZone { get; }
        string IssuerTimeZoneInfo { get; }
        string IssuerLocale { get; }
        string IssuerLocaleInfo { get; }
        string IssuerClockify { get; }
        string IssuerLogoFileName { get; }
        string TableName { get; }
        string TableContact { get; }
        string TableCurrency { get; }
        string ValidationLowerThan0 { get; }
    }
    public class Issuers_en : Standard_en, IIssuers
    {
        public string PageTitle => "Issuers";
        public string AddTitle => "Add Issuer";
        public string EditTitle => "Edit";
        public string Info => "Select Issuer to edit or add.";
        public string IssuerName => "Name";
        public string IssuerEmployee => "Invoice issued by";
        public string IssuerEmployeeInfo => "Emploeyee, place";
        public string IssuerCurrency => "Currency";
        public string IssuerLimit => "Limit";
        public string IssuerContact => "Contact Info";
        public string IssuerContactInfo => "Name, address, VAT ID";
        public string IssuerTimeZone => "Time Zone";
        public string IssuerTimeZoneInfo => "Europe/Zagreb";
        public string IssuerLocale => "Locale";
        public string IssuerLocaleInfo => "hr-HR";
        public string IssuerClockify => "Clockify detailed url";
        public string IssuerLogoFileName => "Logo file name";
        public string TableName => "Name";
        public string TableContact => "Contact Info";
        public string TableCurrency => "Currency";
        public string ValidationLowerThan0 => "Cannot be lower than 0";
    }
    public class Issuers_hr : Standard_hr, IIssuers
    {
        public string PageTitle => "Izdavači";
        public string AddTitle => "Dodaj izdavača";
        public string EditTitle => "Izmijeni izdavača";
        public string Info => "Odaberi izdavača za izmjenu ili dodaj novog.";
        public string IssuerName => "Naziv";
        public string IssuerEmployee => "Izdano";
        public string IssuerEmployeeInfo => "Djelatnik, mjesto";
        public string IssuerCurrency => "Valuta";
        public string IssuerLimit => "Limit";
        public string IssuerContact => "Kontakt podaci";
        public string IssuerContactInfo => "Naziv, adresa, PDV ID";
        public string IssuerTimeZone => "Vremenska zona";
        public string IssuerTimeZoneInfo => "Europe/Zagreb";
        public string IssuerLocale => "Lokalitet";
        public string IssuerLocaleInfo => "hr-HR";
        public string IssuerClockify => "Detaljna Clockify poveznica";
        public string IssuerLogoFileName => "Naziv logo datoteke";
        public string TableName => "Naziv";
        public string TableContact => "Kontakt Podaci";
        public string TableCurrency => "Valuta";
        public string ValidationLowerThan0 => "Ne može biti manje od 0";
    }
}