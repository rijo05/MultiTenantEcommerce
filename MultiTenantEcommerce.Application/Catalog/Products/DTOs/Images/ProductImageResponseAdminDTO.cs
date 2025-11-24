using MultiTenantEcommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
public class ProductImageResponseAdminDTO : IProductImageDTO
{
    public string Key { get; }
    public string PresignUrl { get; }
    public bool IsMain { get; }
    public string FileName { get; private set; }
    public UploadStatus Status { get; private set; }
    public long Size { get; private set; }
    public string ContentType { get; private set; }
}
