using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Commands.Create;

public record CreateProductCommand(
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId,
    bool? IsActive) : ICommand<ProductResponseAdminDTO>;