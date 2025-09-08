using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Queries.GetById;
public record GetCartByIdQuery(
    Guid CartId) : IQuery<CartResponseDTO>;
