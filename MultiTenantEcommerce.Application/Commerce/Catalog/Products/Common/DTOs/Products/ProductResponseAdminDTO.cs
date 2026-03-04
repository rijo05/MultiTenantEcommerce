using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Images;
using MultiTenantEcommerce.Application.Logistics.Inventory.Common.DTOs;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Enums;
using MultiTenantEcommerce.Shared.Integration.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Products;

public record ProductResponseAdminDTO(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    Guid CategoryId,
    StockStatus StockStatus,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    StockProxyDTO Stock,
    List<ProductImageResponseAdminDTO> Images);