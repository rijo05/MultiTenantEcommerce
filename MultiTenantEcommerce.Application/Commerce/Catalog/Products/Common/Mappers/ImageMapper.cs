using MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.DTOs.Images;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.Common.Mappers;

public static class ImageMapper
{
    public static List<ProductImageResponseDTO> ToDTOList(this IEnumerable<ProductImages> productImages)
    {
        return productImages
            .OrderBy(img => img.SortOrder)
            .Select(img => img.ToDTO())
            .ToList();
    }

    public static List<ProductImageResponseAdminDTO> ToDTOAdminList(this IEnumerable<ProductImages> productImages)
    {
        return productImages
            .OrderBy(img => img.SortOrder)
            .Select(img => img.ToDTOAdmin())
            .ToList();
    }

    public static ProductImageResponseDTO ToDTO(this ProductImages img)
    {
        return new ProductImageResponseDTO(
            img.Id,
            img.Url,
            img.SortOrder,
            img.ContentType);
    }

    public static ProductImageResponseAdminDTO ToDTOAdmin(this ProductImages img)
    {
        var baseDto = img.ToDTO();

        return new ProductImageResponseAdminDTO(
            baseDto.Id,
            baseDto.ImageUrl,
            baseDto.SortOrder,
            baseDto.ContentType,
            img.FileName,
            img.Status,
            img.Size);
    }
}