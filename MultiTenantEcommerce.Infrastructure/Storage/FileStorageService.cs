using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
using MultiTenantEcommerce.Application.Common.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Catalog.Entities;

namespace MultiTenantEcommerce.Infrastructure.Storage;
public class FileStorageService : IFileStorageService
{
    private BasicAWSCredentials _credentials;
    private string _serviceURL = "";
    private string _bucketName = "";

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
        var client = new AmazonS3Client();

        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = Uri.UnescapeDataString(key)
        };

        var response = await client.DeleteObjectAsync(deleteRequest);
    }


    public List<PresignedUpload> GenerateUploadUrls(Guid tenantId, Guid productId, List<ProductImages> images)
    {
        var client = ConnectClient();

        var uploads = new List<PresignedUpload>();

        foreach (var img in images)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = img.Key,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(10),
                ContentType = img.ContentType
            };

            var url = client.GetPreSignedURL(request);

            uploads.Add(new PresignedUpload(img.Key, url, img.ContentType));
        }

        return uploads;
    }

    public ProductImageResponse GetImageUrl(ProductImages image)
    {
        var client = ConnectClient();

        var key = Uri.UnescapeDataString(image.Key);

        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(10)
        };

        var presignedUrl = client.GetPreSignedURL(request);

        return new ProductImageResponse
        {
            Key = image.Key,
            ContentType = image.ContentType,
            IsMain = image.IsMain,
            PresignUrl = presignedUrl
        };
    }

    public List<ProductImageResponse> GetImageUrl(List<ProductImages> images)
    {
        var client = ConnectClient();

        var list = new List<ProductImageResponse>();
        foreach (var item in images)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = Uri.UnescapeDataString(item.Key),
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(10)
            };
            var presignedUrl = client.GetPreSignedURL(request);

            list.Add(new ProductImageResponse 
            { 
                Key = item.Key,
                ContentType = item.ContentType,
                IsMain = item.IsMain,
                PresignUrl = presignedUrl
            });
        }

        return list;
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
