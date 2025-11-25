using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using System.Xml;

namespace MultiTenantEcommerce.Application.Catalog.Products.Mappers;
public class ImageMapper
{
    private readonly IFileStorageService _fileStorageService;

    public ImageMapper(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public List<ProductImageResponse> ToProductResponseDTO(List<ProductImages> productImages)
    {
        return _fileStorageService.GetImageUrl(productImages);
    }

    public List<ProductImageResponseAdminDTO> ToProductResponseAdminDTO(List<ProductImages> productImages)
    {
        var images = _fileStorageService.GetImageUrl(productImages);

        List<ProductImageResponseAdminDTO> lista = new List<ProductImageResponseAdminDTO>();

        var dt = images.ToDictionary(s => s.Key);

        foreach (var image in productImages)
        {
            var hasUrl = dt.TryGetValue(image.Key, out var urlDto);

            lista.Add(new ProductImageResponseAdminDTO 
            {
                PresignUrl = hasUrl ? urlDto.PresignUrl : null,

                ContentType = image.ContentType,
                Key = image.Key,
                IsMain = image.IsMain,
                FileName = image.FileName,
                Size = image.Size,
                Status = image.Status               
            });
        }

        return lista;
    }
}
