using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
public class ProductImageResponseDTO : IProductImageDTO
{
    public string Key { get; init; }
    public string PresignUrl { get; init; }
    public bool IsMain { get; init; }
    public string ContentType { get; init; }
}
