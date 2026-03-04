using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Commands.MergeCart;
public record MergeCartCommand(Guid AnonymousId, Guid CustomerId) : ICommand<Unit>;
