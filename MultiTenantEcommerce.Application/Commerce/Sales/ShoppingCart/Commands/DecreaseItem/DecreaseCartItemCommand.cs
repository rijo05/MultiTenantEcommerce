using MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Commands.DecreaseItem;

public record DecreaseCartItemCommand(
    Guid? CustomerId,
    Guid? AnonymousId,
    Guid ProductId,
    int Quantity) : ICommand<CartSummaryDTO>;