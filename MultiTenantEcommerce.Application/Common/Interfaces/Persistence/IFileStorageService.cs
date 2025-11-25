using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Domain.Catalog.Entities;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
public interface IFileStorageService
{
    public List<PresignedUpload> GenerateUploadUrls(Guid tenantId, Guid productId, List<ProductImages> images);
    public ProductImageResponse GetImageUrl(ProductImages key);
    public Task DeleteImageUrl(string key);
    public List<ProductImageResponse> GetImageUrl(List<ProductImages> key);
    public Task UploadAsync(string key, byte[] content, string contentType);
}
