using MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Commands.AddItem;

public record AddCartItemCommand(
    Guid? CustomerId,
    Guid? AnonymousId,
    Guid ProductId,
    int Quantity) : ICommand<CartSummaryDTO>;