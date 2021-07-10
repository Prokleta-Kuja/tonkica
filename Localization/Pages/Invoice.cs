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
        string Print { get; }
        string Clockify { get; }
        string TableTitle { get; }
        string TableQty { get; }
        string TablePrice { get; }
        string TableTotal { get; }
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
        public string Print => "Print";
        public string Clockify => "Load clockify";
        public string TableTitle => "Title";
        public string TableQty => "Qty";
        public string TablePrice => "Price";
        public string TableTotal => "Total";
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
        public string Print => "Ispis";
        public string Clockify => "Učitaj clockify";
        public string TableTitle => "Stavka";
        public string TableQty => "Kol";
        public string TablePrice => "Cijena";
        public string TableTotal => "Ukupno";
    }
}