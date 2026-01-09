using AcheSAP.Application.DTOs;
using AcheSAP.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcheSAP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesOrderController : ControllerBase
    {
        private readonly ISalesOrderService _salesOrderService;
        private readonly ILogger<SalesOrderController> _logger;

        public SalesOrderController(
            ISalesOrderService salesOrderService,
            ILogger<SalesOrderController> logger)
        {
            _salesOrderService = salesOrderService;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os pedidos de venda
        /// </summary>
        /// <returns>Lista de pedidos</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SalesOrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllSalesOrders()
        {
            _logger.LogInformation("Getting all sales orders");

            var orders = await _salesOrderService.GetAllSalesOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Obtém um pedido de venda específico por número
        /// </summary>
        /// <param name="orderNumber">Número do pedido SAP</param>
        /// <returns>Detalhes do pedido</returns>
        [HttpGet("{orderNumber}")]
        [ProducesResponseType(typeof(SalesOrderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSalesOrder(string orderNumber)
        {
            _logger.LogInformation("Getting sales order {OrderNumber}", orderNumber);

            var order = await _salesOrderService.GetSalesOrderAsync(orderNumber);

            if (order == null)
            {
                return NotFound(new { message = $"Sales order {orderNumber} not found" });
            }

            return Ok(order);
        }

        /// <summary>
        /// Cria um novo pedido de venda no SAP S/4HANA SD
        /// </summary>
        /// <param name="request">Dados do pedido</param>
        /// <returns>Pedido criado</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SalesOrderResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateSalesOrder([FromBody] CreateSalesOrderRequest request)
        {
            try
            {
                _logger.LogInformation("Creating sales order for customer {CustomerCode}", request.CustomerCode);

                var order = await _salesOrderService.CreateSalesOrderAsync(request);

                return CreatedAtAction(
                    nameof(GetSalesOrder),
                    new { orderNumber = order.SalesOrderNumber },
                    order);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error creating sales order");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating sales order");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}

