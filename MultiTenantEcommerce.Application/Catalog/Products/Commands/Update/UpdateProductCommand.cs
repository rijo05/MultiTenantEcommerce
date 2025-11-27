using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Products;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.Update;
public record UpdateProductCommand(
    Guid ProductId,
    string? Name,
    string? Description,
    decimal? Price,
    bool? IsActive,
    Guid? CategoryId) : ICommand<ProductResponseAdminDTO>;
