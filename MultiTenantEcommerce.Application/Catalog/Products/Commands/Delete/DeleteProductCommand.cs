using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.Delete;
public record DeleteProductCommand(
    Guid ProductId) : ICommand<Unit>;
