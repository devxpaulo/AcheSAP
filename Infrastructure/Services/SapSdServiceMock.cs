using AcheSAP.Domain.Entities;
using AcheSAP.Domain.Interfaces;

namespace AcheSAP.Infrastructure.Services
{
    /// <summary>
    /// Mock da integração com SAP S/4HANA SD Module
    /// Em produção, usaria SAP NCo (SAP .NET Connector) ou APIs OData
    /// </summary>
    public class SapSdServiceMock : ISapSdService
    {
        public async Task<SalesOrder> CreateSalesOrderInSapAsync(SalesOrder order)
        {
            // Simulação de chamada ao SAP (delay de rede)
            await Task.Delay(500);

            // Em produção:
            // - Conectaria via RFC/BAPI (BAPI_SALESORDER_CREATEFROMDAT2)
            // - Ou via OData API do S/4HANA
            // - Trataria erros de comunicação, validações SAP, etc.

            Console.WriteLine($"[SAP MOCK] Sales Order {order.SalesOrderNumber} created in SAP SD");

            return order;
        }

        public async Task<SalesOrder?> GetSalesOrderFromSapAsync(string orderNumber)
        {
            // Simulação de consulta ao SAP
            await Task.Delay(300);

            // Em produção: 
            // - Usaria BAPI_SALESORDER_GETDETAIL
            // - Ou GET request para OData API

            Console.WriteLine($"[SAP MOCK] Retrieving Sales Order {orderNumber} from SAP SD");

            return null; // Mock retorna null (não implementado neste exemplo)
        }
    }
}

