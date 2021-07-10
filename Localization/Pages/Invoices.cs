namespace tonkica.Localization
{
    public interface IInvoices : IStandard
    {
        string PageTitle { get; }
        string AddTitle { get; }
        string TableIssuer { get; }
        string TableNo { get; }
        string TableClient { get; }
        string TableSubject { get; }
        string TableTotal { get; }
        string TablePublished { get; }
        string TableStatus { get; }
        string InvoiceIssuer { get; }
        string InvoiceClient { get; }
        string InvoiceSubject { get; }
        string InvoiceSequence { get; }
    }
    public class Invoices_en : Standard_en, IInvoices
    {
        public string PageTitle => "Invoices";
        public string AddTitle => "Add new invoice";
        public string TableIssuer => "Issuer";
        public string TableNo => "#";
        public string TableClient => "Client";
        public string TableSubject => "Subject";
        public string TableTotal => "Total";
        public string TablePublished => "Published";
        public string TableStatus => "Status";
        public string InvoiceIssuer => "Issuer";
        public string InvoiceClient => "Client";
        public string InvoiceSubject => "Subject";
        public string InvoiceSequence => "Sequence #";
    }
    public class Invoices_hr : Standard_hr, IInvoices
    {
        public string PageTitle => "Računi";
        public string AddTitle => "Novi račun";
        public string TableIssuer => "Izdavač";
        public string TableNo => "Br";
        public string TableClient => "Klijent";
        public string TableSubject => "Predmet";
        public string TableTotal => "Ukupno";
        public string TablePublished => "Izdano";
        public string TableStatus => "Stanje";
        public string InvoiceIssuer => "Izdavač";
        public string InvoiceClient => "Klijent";
        public string InvoiceSubject => "Predmet";
        public string InvoiceSequence => "Slijedni broj";
    }
}