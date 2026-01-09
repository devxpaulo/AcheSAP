# SAP S/4HANA Sales Integration API
API REST desenvolvida em .NET 8 (LTS) para integração com o módulo SD (Sales & Distribution) do SAP S/4HANA, permitindo a criação e consulta de pedidos de venda através de endpoints seguros e documentados.

## Boas Práticas Aplicadas no Projeto
- Clean Architecture com separação em camadas bem definidas
- Domain-Driven Design (DDD) com entidades ricas e validações de negócio
- Princípios SOLID aplicados em toda a arquitetura
- Autenticação JWT para segurança dos endpoints
- Documentação Swagger/OpenAPI interativa
- Mock de integração SAP simulando chamadas OData
- Repository Pattern para abstração de persistência

## Arquitetura
- A aplicação segue os princípios de Clean Architecture e DDD, organizada em 4 camadas:
  ````
  ┌─────────────────────────────────────────────┐
  │           API Layer (Presentation)          │
  │   Controllers, Middlewares, Swagger         │
  └──────────────────┬──────────────────────────┘
                     │
  ┌──────────────────▼──────────────────────────┐
  │         Application Layer (Use Cases)       │
  │   Services, DTOs, Interfaces                │
  └──────────────────┬──────────────────────────┘
                     │
  ┌──────────────────▼──────────────────────────┐
  │    Infrastructure Layer (External Concerns) │
  │   Repositories, SAP Service, JWT, Data      │
  └──────────────────┬──────────────────────────┘
                     │
  ┌──────────────────▼──────────────────────────┐
  │         Domain Layer (Business Logic)       │
  │   Entities, Value Objects, Domain Services  │
  └─────────────────────────────────────────────┘
## Princípios SOLID Aplicados
- Single Responsibility Principle (SRP)
  - Cada classe tem uma única responsabilidade:
    - SalesOrder → gerencia pedidos
    - SalesOrderService → orquestra casos de uso
    - SalesOrdersController → processa requisições HTTP

- Open/Closed Principle (OCP)
  - Código aberto para extensão, fechado para modificação:
    ````
    // Fácil adicionar novos status sem modificar a entidade
    public enum OrderStatus { Created, InProcess, Completed, Cancelled, OnHold }
- Liskov Substitution Principle (LSP)
  - Interfaces permitem substituição de implementações:
    ````
    // Pode trocar InMemory por SQL Server sem quebrar o código
    ISalesOrderRepository repository = new SqlSalesOrderRepository();
- Interface Segregation Principle (ISP)
  - Interfaces pequenas e específicas:
    ````
    public interface ISalesOrderRepository { ... }
    public interface ISapS4HanaService { ... }
    public interface IJwtTokenService { ... }
- Dependency Inversion Principle (DIP)
  - Camadas dependem de abstrações:
    ````
    // SalesOrderService depende de interfaces, não implementações concretas
    public SalesOrderService(
    ISalesOrderRepository repository,
    ISapS4HanaService sapService) { }
## Integração SAP S/4HANA
- Arquitetura de Integração
  ````
  ┌─────────────┐      HTTP/OData      ┌──────────────────┐
  │   API .NET  │ ←──────────────────→ │  SAP S/4HANA     │
  │             │     JSON Payload     │  SD Module       │
  └─────────────┘                      └──────────────────┘
- Mock vs Produção
  - Mock (Desenvolvimento):
    ````
    public class MockSapS4HanaService : ISapS4HanaService
    {
        // Simula latência e armazenamento em memória
        public async Task<SalesOrder> SendOrderToSapAsync(SalesOrder order)
        {
            Thread.Sleep(100); // Simula latência de rede
            _sapOrders[order.OrderNumber] = order;
            return order;
        }
    }
  - Produção (OData):
    ````
    public class SapS4HanaODataService : ISapS4HanaService
    {
        public async Task<SalesOrder> SendOrderToSapAsync(SalesOrder order)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/sap/opu/odata4/sap/api_sales_order_srv/srvd_a2x/sap/salesorder/0001/SalesOrder",
                CreateODataPayload(order)
            );
            // Processar resposta...
        }
    }
- Entidades SAP Mapeadas
  ````
  Entidade .NET	  | Tabela SAP	 | Entity Set OData
  SalesOrder	      | VBAK	     | A_SalesOrder
  SalesOrderItem	  | VBAP	     | A_SalesOrderItem
## API Endpoints
-  Autenticação
   - Obter Token JWT
     ````
     POST /api/auth/login
     {
        "username": "admin",
        "password": "admin123"
     }
-  Pedidos de Venda
   - Criar Pedido
     ````
     POST /api/salesorders
   - Listar Todos os Pedidos
     ````
     GET /api/salesorders
   - Consultar Pedido Específico
     ````
     GET /api/salesorders/{orderNumber}
## Tecnologias Utilizadas
   ````
   Tecnologia	    | Versão	 | Propósito
   .NET	        | 8.0 LTS	 | Framework principal
   C#	            | 12.0	     | Linguagem de programação
   ASP.NET Core	| 8.0	     | Web API framework
   Swashbuckle	    | 6.5.0	     | Documentação OpenAPI/Swagger
   JWT Bearer	    | 8.0.0	     | Autenticação JWT
   ````
## Uso de IA no desenvolvimento do Projeto
-  Claude 4.5 Sonnet Thinking
   - Foi utlizado o Claude Sonnet como auxiliar na aplicação dos padrões de projeto e modelagem das camadas da aplicação, bem como na documentação final.
   - O uso da IA foi importante como prática atual de desenvolvimento de soluções e resolução de desafios técnicos.  

## Autor
-  Paulo Amorim - Desenvolvedor .NET
-  📧 Email: devxpaulo@gmail.com
-  💼 LinkedIn: linkedin.com/in/devxpaulo
