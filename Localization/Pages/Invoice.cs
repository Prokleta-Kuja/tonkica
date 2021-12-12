namespace tonkica.Localization
{
    public interface IInvoice : IStandard
    {
        string PageTitle { get; }
        string InvoiceIssuer { get; }
        string InvoiceClient { get; }
        string InvoiceSubject { get; }
        string InvoiceSequence { get; }
        string InvoiceCurrency { get; }
        string InvoiceDisplayCurrency { get; }
        string InvoiceIssuerCurrency { get; }
        string InvoiceStatus { get; }
        string InvoicePublished { get; }
        string InvoiceNote { get; }
        string Clockify { get; }
        string ClockifyStart { get; }
        string ClockifyEnd { get; }
        string ClockifyHours { get; }
        string ClockifyItems { get; }
        string ClockifyLoad { get; }
        string TableTitle { get; }
        string TableQty { get; }
        string TablePrice { get; }
        string TableTotal { get; }
        string ValidationCannotBeInTheFuture { get; }
    }
    public class Invoice_en : Standard_en, IInvoice
    {
        public string PageTitle => "Invoice";
        public string InvoiceIssuer => "Issuer";
        public string InvoiceClient => "Client";
        public string InvoiceSubject => "Subject";
        public string InvoiceSequence => "Sequence #";
        public string InvoiceCurrency => "Invoice Currency";
        public string InvoiceDisplayCurrency => "Display Currency";
        public string InvoiceIssuerCurrency => "Issuer Currency";
        public string InvoiceStatus => "Status";
        public string InvoicePublished => "Published";
        public string InvoiceNote => "Note";
        public string Clockify => "Clockify";
        public string ClockifyStart => "Start";
        public string ClockifyEnd => "End";
        public string ClockifyHours => "Hours:";
        public string ClockifyItems => "Items:";
        public string ClockifyLoad => "Load";
        public string TableTitle => "Title";
        public string TableQty => "Qty";
        public string TablePrice => "Price";
        public string TableTotal => "Total";
        public string ValidationCannotBeInTheFuture => "Cannot be in the future";
    }
    public class Invoice_hr : Standard_hr, IInvoice
    {
        public string PageTitle => "Račun";
        public string InvoiceIssuer => "Izdavač";
        public string InvoiceClient => "Klijent";
        public string InvoiceSubject => "Predmet";
        public string InvoiceSequence => "Slijedni broj";
        public string InvoiceCurrency => "Valuta računa";
        public string InvoiceDisplayCurrency => "Valuta za prikaz";
        public string InvoiceIssuerCurrency => "Valuta izdavača";
        public string InvoiceStatus => "Stanje";
        public string InvoicePublished => "Izdano";
        public string InvoiceNote => "Napomena";
        public string Clockify => "Clockify";
        public string ClockifyStart => "Od";
        public string ClockifyEnd => "Do";
        public string ClockifyHours => "Sati:";
        public string ClockifyItems => "Stavki:";
        public string ClockifyLoad => "Učitaj";
        public string TableTitle => "Stavka";
        public string TableQty => "Kol";
        public string TablePrice => "Cijena";
        public string TableTotal => "Ukupno";
        public string ValidationCannotBeInTheFuture => "Ne može biti u budućnosti";
    }
}