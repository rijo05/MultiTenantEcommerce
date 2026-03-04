using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.DTOs;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetByIdAdmin;

public record GetOrderByIdAdminQuery(
    Guid? CustomerId,
    Guid OrderId) : IQuery<OrderDetailAdminDTO>;