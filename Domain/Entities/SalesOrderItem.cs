namespace AcheSAP.Domain.Entities
{
    /// <summary>
    /// Item de um Pedido de Venda (linha do pedido)
    /// </summary>
    public class SalesOrderItem
    {
        public int ItemNumber { get; private set; }
        public string MaterialCode { get; private set; }
        public string MaterialDescription { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string Unit { get; private set; }

        private SalesOrderItem() { }

        public static SalesOrderItem Create(
            int itemNumber,
            string materialCode,
            string materialDescription,
            int quantity,
            decimal unitPrice,
            string unit = "UN")
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));

            if (unitPrice <= 0)
                throw new ArgumentException("Unit price must be greater than zero", nameof(unitPrice));

            var item = new SalesOrderItem
            {
                ItemNumber = itemNumber,
                MaterialCode = materialCode,
                MaterialDescription = materialDescription,
                Quantity = quantity,
                UnitPrice = unitPrice,
                Unit = unit
            };

            item.CalculateTotalPrice();
            return item;
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = Quantity * UnitPrice;
        }
    }
}
