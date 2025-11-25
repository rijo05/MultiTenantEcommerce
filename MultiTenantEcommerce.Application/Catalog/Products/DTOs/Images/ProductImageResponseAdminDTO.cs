using MultiTenantEcommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
public class ProductImageResponseAdminDTO : IProductImageDTO
{
    public string Key { get; init; }
    public string PresignUrl { get; init; }
    public bool IsMain { get; init; }
    public string FileName { get; init; }
    public UploadStatus Status { get; init; }
    public long Size { get; init; }
    public string ContentType { get; init; }
}
