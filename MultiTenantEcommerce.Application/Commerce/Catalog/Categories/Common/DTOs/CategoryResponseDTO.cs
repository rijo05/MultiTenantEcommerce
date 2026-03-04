namespace MultiTenantEcommerce.Application.Commerce.Catalog.Categories.Common.DTOs;

public record CategoryResponseDTO(
    Guid Id,
    string Name,
    string? Description);