using MultiTenantEcommerce.Application.Catalog.Products.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Catalog.Products.Commands.Create;
public record CreateProductCommand(
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId,
    bool? IsActive,
    int? Quantity,
    int? MinimumQuantity) : ICommand<ProductResponseAdminDTO>;
