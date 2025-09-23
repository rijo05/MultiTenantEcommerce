using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.RemoveItem;
public record DecreaseCartItemCommand(
    Guid CustomerId,
    Guid ProductId,
    int Quantity) : ICommand<CartResponseDTO>;
