using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Catalog.Entities;
public class ProductImages : TenantBase
{
    public Guid ProductId { get; set; }
    public string Key { get; private set; }
    public string FileName { get; private set; }
    public bool IsMain { get; private set; }
    public UploadStatus Status { get; private set; }
    public long Size { get; private set; }
    public string ContentType { get; private set; }

    private ProductImages() { }
    internal ProductImages(Guid tenantId, Guid productId, string fileName, long size, string contentType, bool isMain = false)
        : base(tenantId)
    {
        ProductId = productId;
        FileName = fileName;
        Size = size;
        ContentType = contentType;
        IsMain = isMain;
        Status = UploadStatus.Pending;

        Key = $"{tenantId}/products/{productId}/{this.Id}";
    }

    internal void UnmarkAsMain() => IsMain = false;

    internal void MarkAsMain() => IsMain = true;

    internal void MarkAsCompleted() => Status = UploadStatus.Completed;
}
