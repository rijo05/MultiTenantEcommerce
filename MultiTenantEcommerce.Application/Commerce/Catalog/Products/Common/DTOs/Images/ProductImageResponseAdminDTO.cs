using MultiTenantEcommerce.Domain.Commerce.Catalog.Enums;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Images;

public record ProductImageResponseAdminDTO(
    Guid Id,
    string ImageUrl,
    int SortOrder,
    string ContentType,
    string FileName,
    UploadStatus Status,
    long Size);