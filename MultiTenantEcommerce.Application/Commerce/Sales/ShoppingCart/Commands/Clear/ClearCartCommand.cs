using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Commands.Clear;

public record ClearCartCommand(Guid? CustomerId, Guid? AnonymousId) : ICommand<Unit>;