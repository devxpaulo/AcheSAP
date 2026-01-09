namespace AcheSAP.Application.DTOs
{
    public class CreateSalesOrderRequest
    {
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Currency { get; set; } = "BRL";
        public List<SalesOrderItemDto> Items { get; set; } = new();
    }

    public class SalesOrderItemDto
    {
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialDescription { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Unit { get; set; } = "UN";
    }
}

