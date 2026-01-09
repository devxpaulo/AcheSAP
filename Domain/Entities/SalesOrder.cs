namespace AcheSAP.Domain.Entities
{
    /// <summary>
    /// Entidade de Domínio representando um Pedido de Venda (Sales Order) do SAP SD
    /// </summary>
    public class SalesOrder
    {
        public string SalesOrderNumber { get; private set; }
        public DateTime OrderDate { get; private set; }
        public string CustomerCode { get; private set; }
        public string CustomerName { get; private set; }
        public decimal TotalAmount { get; private set; }
        public string Currency { get; private set; }
        public SalesOrderStatus Status { get; private set; }
        public List<SalesOrderItem> Items { get; private set; }

        // Construtor privado para garantir consistência (DDD)
        private SalesOrder()
        {
            Items = new List<SalesOrderItem>();
        }

        // Factory Method (padrão de criação DDD)
        public static SalesOrder Create(
            string customerCode,
            string customerName,
            string currency = "BRL")
        {
            if (string.IsNullOrWhiteSpace(customerCode))
                throw new ArgumentException("Customer code is required", nameof(customerCode));

            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Customer name is required", nameof(customerName));

            return new SalesOrder
            {
                SalesOrderNumber = GenerateOrderNumber(),
                OrderDate = DateTime.UtcNow,
                CustomerCode = customerCode,
                CustomerName = customerName,
                Currency = currency,
                Status = SalesOrderStatus.Created,
                Items = new List<SalesOrderItem>()
            };
        }

        public void AddItem(SalesOrderItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Items.Add(item);
            RecalculateTotalAmount();
        }

        private void RecalculateTotalAmount()
        {
            TotalAmount = Items.Sum(i => i.TotalPrice);
        }

        public void Confirm()
        {
            if (!Items.Any())
                throw new InvalidOperationException("Cannot confirm order without items");

            Status = SalesOrderStatus.Confirmed;
        }

        private static string GenerateOrderNumber()
        {
            // Simulação do padrão SAP: 10 dígitos numéricos
            return $"40{DateTime.UtcNow:yyyyMMddHHmmss}".Substring(0, 10);
        }
    }

    public enum SalesOrderStatus
    {
        Created = 0,
        Confirmed = 1,
        Processing = 2,
        Delivered = 3,
        Cancelled = 4
    }
}

