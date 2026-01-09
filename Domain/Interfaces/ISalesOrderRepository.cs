using AcheSAP.Domain.Entities;

namespace AcheSAP.Domain.Interfaces
{
    /// <summary>
    /// Interface do repositório seguindo Repository Pattern
    /// </summary>
    public interface ISalesOrderRepository
    {
        Task<SalesOrder?> GetByOrderNumberAsync(string orderNumber);
        Task<IEnumerable<SalesOrder>> GetAllAsync();
        Task<SalesOrder> CreateAsync(SalesOrder salesOrder);
    }
}
