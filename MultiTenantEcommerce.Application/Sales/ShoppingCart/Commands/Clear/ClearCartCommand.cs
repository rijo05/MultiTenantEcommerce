using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Clear;
public record ClearCartCommand(
    Guid CustomerId) : ICommand<Unit>;
