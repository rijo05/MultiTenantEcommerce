using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.RemoveItem;
public record RemoveCartItemCommand(
    Guid CustomerId,
    Guid ProductId) : ICommand<CartResponseDTO>;
