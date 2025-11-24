using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Domain.Catalog.Entities;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
public interface IFileStorageService
{
    public List<PresignedUpload> GenerateUploadUrls(Guid tenantId, Guid productId, List<ProductImages> images);
    public string GetImageUrl(string key);
    public Task DeleteImageUrl(string key);
    public List<string> GetImageUrl(List<string> key);
    public Task UploadAsync(string key, byte[] content, string contentType);
}
