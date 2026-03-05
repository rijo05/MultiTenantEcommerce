using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;
using MultiTenantEcommerce.Domain.Commerce.Catalog.ValueObjects;
using MultiTenantEcommerce.Shared.Application.Interfaces;
using MultiTenantEcommerce.Shared.Integration.DTOs;

namespace MultiTenantEcommerce.Infrastructure.Shared.Storage;

public class FileStorageService : IFileStorageService
{
    private readonly string _bucketName = "";
    private readonly BasicAWSCredentials _credentials;
    private readonly string _serviceURL = "";

    public FileStorageService(IConfiguration configuration)
    {
        var acessKey = configuration["Cloudflare:AccessKeyId"];
        var secretKey = configuration["Cloudflare:SecretAccessKey"];
        _serviceURL = configuration["Cloudflare:ServiceUrl"];
        _bucketName = configuration["Cloudflare:BucketName"];

        _credentials = new BasicAWSCredentials(acessKey, secretKey);
    }

    public async Task DeleteImageUrl(string key)
    {
        var client = ConnectClient();

        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = Uri.UnescapeDataString(key)
        };

        var response = await client.DeleteObjectAsync(deleteRequest);
    }


    public List<PresignedUpload> GenerateUploadUrls(Guid tenantId, Guid productId, List<(string key, string contentType)> values)
    {
        var client = ConnectClient();

        var uploads = new List<PresignedUpload>();

        foreach (var img in values)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = Uri.UnescapeDataString(img.key),
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(10),
                ContentType = img.contentType
            };

            var url = client.GetPreSignedURL(request);

            uploads.Add(new PresignedUpload(img.key, url, img.contentType));
        }

        return uploads;
    }

    public Dictionary<string, string> GetPresignedUrls(List<string> keys)
    {
        var client = ConnectClient();

        var urls = new Dictionary<string, string>();

        foreach (var item in keys)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = Uri.UnescapeDataString(item),
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(60)
            };
            urls[item] = client.GetPreSignedURL(request);
        }

        return urls;
    }

    public async Task UploadAsync(string key, byte[] content, string contentType)
    {
        var client = ConnectClient();

        using var stream = new MemoryStream(content);

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            AutoCloseStream = true,
            InputStream = stream,
            ContentType = contentType,
            Key = key
        };

        await client.PutObjectAsync(request);
    }

    private AmazonS3Client ConnectClient()
    {
        return new AmazonS3Client(_credentials, new AmazonS3Config
        {
            ServiceURL = _serviceURL,
            ForcePathStyle = true,
            AuthenticationRegion = "auto"
        });
    }
}