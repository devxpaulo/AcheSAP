using AcheSAP.Domain.Entities;
using AcheSAP.Domain.Interfaces;

namespace AcheSAP.Infrastructure.Repositories
{
    /// <summary>
    /// Repositório In-Memory (mock)
    /// Em produção usaria Entity Framework Core com SQL Server/HANA
    /// </summary>
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly List<SalesOrder> _orders = new();
        private readonly object _lock = new();

        public Task<SalesOrder?> GetByOrderNumberAsync(string orderNumber)
        {
            lock (_lock)
            {
                var order = _orders.FirstOrDefault(o => o.SalesOrderNumber == orderNumber);
                return Task.FromResult(order);
            }
        }

        public Task<IEnumerable<SalesOrder>> GetAllAsync()
        {
            lock (_lock)
            {
                return Task.FromResult<IEnumerable<SalesOrder>>(_orders.ToList());
            }
        }

        public Task<SalesOrder> CreateAsync(SalesOrder salesOrder)
        {
            lock (_lock)
            {
                _orders.Add(salesOrder);
                return Task.FromResult(salesOrder);
            }
        }
    }
}

