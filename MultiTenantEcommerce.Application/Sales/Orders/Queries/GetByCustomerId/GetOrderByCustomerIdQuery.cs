using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;

namespace MultiTenantEcommerce.Application.Sales.Orders.Queries.GetByCustomerId;
public record GetOrderByCustomerIdQuery(
    Guid customerId) : IQuery<List<OrderResponseDTO>>;
