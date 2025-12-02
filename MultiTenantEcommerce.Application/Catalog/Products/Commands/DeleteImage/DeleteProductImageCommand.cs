using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.DeleteImage;
public record DeleteProductImageCommand(
    Guid ProductId,
    Guid ImageId) : ICommand<Unit>;
