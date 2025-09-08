using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Queries.GetByCustomerId;
public record GetCartByCustomerIdQuery(
    Guid CustomerId) : IQuery<CartResponseDTO>;
