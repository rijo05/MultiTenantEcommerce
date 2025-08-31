using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.Update;
public record UpdateProductCommand(
    Guid ProductId,
    string? Name,
    string? Description,
    decimal? Price,
    bool? IsActive,
    Guid? CategoryId) : ICommand<ProductResponseDTO>;
