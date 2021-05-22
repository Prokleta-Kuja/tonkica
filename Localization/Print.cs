namespace tonkica.Localization
{
    public interface IPrint
    {
        string Invoice { get; }
        string Issued { get; }
        string IssuedByEmployee { get; }
        string Delivered { get; }
        string DueBy { get; }
        string IssuedTo { get; }
        string IssuedByIssuer { get; }
        string PaymentDetails { get; }
        string No { get; }
        string Description { get; }
        string Cost { get; }
        string Quantity { get; }
        string Amount { get; }
        string Currency { get; }
        string Rate { get; }
        string Total { get; }
        string Note { get; }
    }
    public class Print_en : IPrint
    {
        public string Invoice => "Invoice";
        public string Issued => "Issued";
        public string IssuedByEmployee => "Issued by";
        public string Delivered => "Delivered";
        public string DueBy => "Due by";
        public string IssuedTo => "Issued To";
        public string IssuedByIssuer => "Issued By";
        public string PaymentDetails => "Payment Details";
        public string No => "No";
        public string Description => "Description";
        public string Cost => "Cost";
        public string Quantity => "QTY";
        public string Amount => "Amount";
        public string Currency => "Currency";
        public string Rate => "Rate";
        public string Total => "Total";
        public string Note => "Note";
    }
    public class Print_hr : IPrint
    {
        public string Invoice => "Račun";
        public string Issued => "Izdano";
        public string IssuedByEmployee => "Izdao";
        public string Delivered => "Isporučeno";
        public string DueBy => "Dospijeće";
        public string IssuedTo => "Kupac";
        public string IssuedByIssuer => "Isporučitelj";
        public string PaymentDetails => "Plaćanje";
        public string No => "R.Br.";
        public string Description => "Opis";
        public string Cost => "Cijena";
        public string Quantity => "Kol.";
        public string Amount => "Iznos";
        public string Currency => "Valuta";
        public string Rate => "Tečaj";
        public string Total => "Ukupno";
        public string Note => "Napomena";
    }
}