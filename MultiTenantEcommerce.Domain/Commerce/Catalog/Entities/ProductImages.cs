using MultiTenantEcommerce.Domain.Commerce.Catalog.Enums;
using MultiTenantEcommerce.Shared.Domain.Abstractions;

namespace MultiTenantEcommerce.Domain.Commerce.Catalog.Entities;

public class ProductImages : TenantBase
{
    private ProductImages()
    {
    }

    internal ProductImages(Guid tenantId, Guid productId, string fileName, long size, string contentType, int sortOrder)
        : base(tenantId)
    {
        ProductId = productId;
        FileName = fileName;
        Size = size;
        ContentType = contentType;
        SortOrder = sortOrder;
        Status = UploadStatus.Pending;

        Key = $"{tenantId}/products/{productId}/{Id}";

        Url = $"https://cdn.plataforma.com/product-images/{Key}";
    }

    public Guid ProductId { get; set; }
    public string Key { get; private set; }
    public string Url { get; private set; }
    public string FileName { get; private set; }
    public int SortOrder { get; private set; }
    public UploadStatus Status { get; private set; }
    public long Size { get; private set; }
    public string ContentType { get; private set; }

    internal void ChangeOrder(int newOrder)
    {
        SortOrder = newOrder;
    }

    internal void MarkAsCompleted()
    {
        Status = UploadStatus.Completed;
    }
}