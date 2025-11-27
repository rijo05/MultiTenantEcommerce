using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Enums;
using System.Xml;

namespace MultiTenantEcommerce.Application.Catalog.Products.Mappers;
public class ImageMapper
{
    public List<IProductImageDTO> ToProductImageResponseDTO(IEnumerable<ProductImages> productImages, Dictionary<string, string> signedUrls)
    {
        var list = new List<IProductImageDTO>();

        foreach (var img in productImages)
        {
            if (img.Status != UploadStatus.Completed) continue;

            var url = signedUrls.TryGetValue(img.Key, out var signedUrl) ? signedUrl : null;

            list.Add(new ProductImageResponseDTO
            {
                Key = img.Key,
                PresignUrl = url,
                IsMain = img.IsMain,
                ContentType = img.ContentType
            });
        }
        return list;
    }

    public List<IProductImageDTO> ToProductImageResponseAdminDTO(IEnumerable<ProductImages> productImages, Dictionary<string, string> signedUrls)
    {
        var list = new List<IProductImageDTO>();

        foreach (var img in productImages)
        {
            var url = signedUrls.TryGetValue(img.Key, out var signedUrl) ? signedUrl : null;

            list.Add(new ProductImageResponseAdminDTO
            {
                Key = img.Key,
                PresignUrl = url,
                IsMain = img.IsMain,
                ContentType = img.ContentType,
                FileName = img.FileName,
                Size = img.Size,
                Status = img.Status
            });
        }
        return list;
    }
}
