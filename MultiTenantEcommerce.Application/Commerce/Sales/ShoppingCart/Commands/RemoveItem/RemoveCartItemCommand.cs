using MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Commands.RemoveItem;

public record RemoveCartItemCommand(Guid? CustomerId, 
    Guid? AnonymousId, 
    Guid ProductId) : ICommand<CartSummaryDTO>;