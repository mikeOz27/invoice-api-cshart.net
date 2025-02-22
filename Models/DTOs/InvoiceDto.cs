namespace YourNamespace.Models.DTOs
{
    public class InvoiceDto
    {
        public string Customer { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public List<InvoiceDetailDto> Details { get; set; }
    }

    public class InvoiceDetailDto
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
