namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Images;

public record ProductImageResponseDTO(
    Guid Id, 
    string ImageUrl, 
    int SortOrder, 
    string ContentType);