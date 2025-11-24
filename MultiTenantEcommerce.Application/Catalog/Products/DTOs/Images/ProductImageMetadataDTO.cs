namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
public record ProductImageMetadataDTO(
    string FileName,
    long Size,
    string ContentType,
    bool IsMain);
