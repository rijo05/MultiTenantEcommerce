using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Domain.Catalog.Entities;

namespace MultiTenantEcommerce.Application.Catalog.Products.Mappers;
public class ImageMapper
{
    public List<ProductImageResponseDTO> ToProductImageResponseDTOList(IEnumerable<ProductImages> productImages, Dictionary<string, string> signedUrls)
    {
        return productImages
            .Select(img => ToProductImageResponseDTO(img, signedUrls))
            .ToList();
    }

    public List<ProductImageResponseAdminDTO> ToProductImageResponseAdminDTOList(IEnumerable<ProductImages> productImages, Dictionary<string, string> signedUrls)
    {
        return productImages
            .Select(img => ToProductImageResponseAdminDTO(img, signedUrls))
            .ToList();
    }

    public ProductImageResponseDTO ToProductImageResponseDTO(ProductImages img, Dictionary<string, string> signedUrls)
    {
        return new ProductImageResponseDTO
        {
            Id = img.Id,
            PresignUrl = signedUrls.TryGetValue(img.Key, out var url) ? url : null,
            IsMain = img.IsMain,
            ContentType = img.ContentType
        };
    }

    public ProductImageResponseAdminDTO ToProductImageResponseAdminDTO(ProductImages img, Dictionary<string, string> signedUrls)
    {
        return new ProductImageResponseAdminDTO
        {
            Id = img.Id,
            PresignUrl = signedUrls.TryGetValue(img.Key, out var url) ? url : null,
            IsMain = img.IsMain,
            ContentType = img.ContentType,
            FileName = img.FileName,
            Size = img.Size,
            Status = img.Status
        };
    }
}
