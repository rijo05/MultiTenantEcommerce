namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;

public record CategoryResponseAdminDTO(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);