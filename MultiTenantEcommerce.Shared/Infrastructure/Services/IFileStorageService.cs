using MultiTenantEcommerce.Shared.Integration.DTOs;

namespace MultiTenantEcommerce.Shared.Infrastructure.Services;

public interface IFileStorageService
{
    public List<PresignedUpload> GenerateUploadUrls(Guid tenantId, Guid productId, List<(string key, string contentType)> values);
    public Task DeleteImageUrl(string key);
    public Dictionary<string, string> GetPresignedUrls(List<string> keys);
    public Task UploadAsync(string key, byte[] content, string contentType);
}