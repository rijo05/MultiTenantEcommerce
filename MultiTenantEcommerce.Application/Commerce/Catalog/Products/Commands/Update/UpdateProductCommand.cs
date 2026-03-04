using MediatR;
using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.Update;

public record UpdateProductCommand(
    Guid ProductId,
    string? Name,
    string? Description,
    decimal? Price,
    bool? IsActive,
    Guid? CategoryId) : ICommand<Unit>;