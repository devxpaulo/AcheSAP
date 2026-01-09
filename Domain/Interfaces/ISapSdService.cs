using AcheSAP.Domain.Entities;

namespace AcheSAP.Domain.Interfaces
{
    /// <summary>
    /// Interface para integração com SAP SD Module
    /// </summary>
    public interface ISapSdService
    {
        Task<SalesOrder> CreateSalesOrderInSapAsync(SalesOrder order);
        Task<SalesOrder?> GetSalesOrderFromSapAsync(string orderNumber);
    }
}

