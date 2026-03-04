using MediatR;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.ManageImages;

public record DeleteProductImageCommand(
    Guid ProductId,
    Guid ImageId) : ICommand<Unit>;