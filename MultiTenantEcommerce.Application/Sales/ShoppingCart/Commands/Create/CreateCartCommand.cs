using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Create;
public record CreateCartCommand(
    Guid CustomerId) : ICommand<CartResponseDTO>;
