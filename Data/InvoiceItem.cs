namespace tonkica.Data
{
    public class InvoiceItem
    {
        public InvoiceItem(string title)
        {
            Title = title;
        }

        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string Title { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }

        public Invoice? Invoice { get; set; }
    }
}