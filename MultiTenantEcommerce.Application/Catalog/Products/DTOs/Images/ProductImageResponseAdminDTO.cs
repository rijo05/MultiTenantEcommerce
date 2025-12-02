using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
public class ProductImageResponseAdminDTO : IProductImageDTO
{
    public Guid Id { get; init; }
    public string PresignUrl { get; init; }
    public bool IsMain { get; init; }
    public string FileName { get; init; }
    public UploadStatus Status { get; init; }
    public long Size { get; init; }
    public string ContentType { get; init; }
}
