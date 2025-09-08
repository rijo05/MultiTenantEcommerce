using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.Delete;
public record DeleteProductCommand(
    Guid ProductId) : ICommand<Unit>;
