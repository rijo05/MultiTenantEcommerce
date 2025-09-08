using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.AddItem;
public record AddCartItemCommand(
    Guid CustomerId,
    Guid ProductId,
    int quantity) : ICommand<CartResponseDTO>;
