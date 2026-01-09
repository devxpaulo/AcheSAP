namespace AcheSAP.Application.DTOs
{
    public class SalesOrderResponse
    {
        public string SalesOrderNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string CustomerCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<SalesOrderItemResponse> Items { get; set; } = new();
    }

    public class SalesOrderItemResponse
    {
        public int ItemNumber { get; set; }
        public string MaterialCode { get; set; } = string.Empty;
        public string MaterialDescription { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}

