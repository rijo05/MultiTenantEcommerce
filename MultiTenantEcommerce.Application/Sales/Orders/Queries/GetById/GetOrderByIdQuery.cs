using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;

namespace MultiTenantEcommerce.Application.Sales.Orders.Queries.GetById;
public record GetOrderByIdQuery(
    Guid OrderId) : IQuery<OrderResponseWithPayment>;
