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
}