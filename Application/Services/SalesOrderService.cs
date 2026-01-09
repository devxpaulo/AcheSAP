using AcheSAP.Application.DTOs;
using AcheSAP.Domain.Entities;
using AcheSAP.Domain.Interfaces;

namespace AcheSAP.Application.Services
{
    /// <summary>
    /// Service da camada de aplicação (Use Case)
    /// Responsabilidade única: Orquestrar a criação de pedidos
    /// </summary>
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ISalesOrderRepository _repository;
        private readonly ISapSdService _sapSdService;

        public SalesOrderService(
            ISalesOrderRepository repository,
            ISapSdService sapSdService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _sapSdService = sapSdService ?? throw new ArgumentNullException(nameof(sapSdService));
        }

        public async Task<SalesOrderResponse> CreateSalesOrderAsync(CreateSalesOrderRequest request)
        {
            // Criação da entidade de domínio
            var salesOrder = SalesOrder.Create(
                request.CustomerCode,
                request.CustomerName,
                request.Currency);

            // Adiciona os itens
            int itemNumber = 10; // SAP usa incrementos de 10
            foreach (var itemDto in request.Items)
            {
                var item = SalesOrderItem.Create(
                    itemNumber,
                    itemDto.MaterialCode,
                    itemDto.MaterialDescription,
                    itemDto.Quantity,
                    itemDto.UnitPrice,
                    itemDto.Unit);

                salesOrder.AddItem(item);
                itemNumber += 10;
            }

            // Confirma o pedido
            salesOrder.Confirm();

            // Integração com SAP (mockado)
            var sapOrder = await _sapSdService.CreateSalesOrderInSapAsync(salesOrder);

            // Persiste no repositório
            await _repository.CreateAsync(sapOrder);

            // Retorna o DTO de resposta
            return MapToResponse(sapOrder);
        }

        public async Task<SalesOrderResponse?> GetSalesOrderAsync(string orderNumber)
        {
            var order = await _repository.GetByOrderNumberAsync(orderNumber);
            return order != null ? MapToResponse(order) : null;
        }

        public async Task<IEnumerable<SalesOrderResponse>> GetAllSalesOrdersAsync()
        {
            var orders = await _repository.GetAllAsync();
            return orders.Select(MapToResponse);
        }

        private static SalesOrderResponse MapToResponse(SalesOrder order)
        {
            return new SalesOrderResponse
            {
                SalesOrderNumber = order.SalesOrderNumber,
                OrderDate = order.OrderDate,
                CustomerCode = order.CustomerCode,
                CustomerName = order.CustomerName,
                TotalAmount = order.TotalAmount,
                Currency = order.Currency,
                Status = order.Status.ToString(),
                Items = order.Items.Select(i => new SalesOrderItemResponse
                {
                    ItemNumber = i.ItemNumber,
                    MaterialCode = i.MaterialCode,
                    MaterialDescription = i.MaterialDescription,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice,
                    Unit = i.Unit
                }).ToList()
            };
        }
    }

    public interface ISalesOrderService
    {
        Task<SalesOrderResponse> CreateSalesOrderAsync(CreateSalesOrderRequest request);
        Task<SalesOrderResponse?> GetSalesOrderAsync(string orderNumber);
        Task<IEnumerable<SalesOrderResponse>> GetAllSalesOrdersAsync();
    }
}

