using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Images;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Enums;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;

public record ProductResponseDTO(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId,
    StockStatus StockStatus,
    List<ProductImageResponseDTO> Images);