using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.Delete;

public record DeleteProductCommand(
    Guid ProductId) : ICommand<Unit>;