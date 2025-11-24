using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
public class ProductImageResponse : IProductImageDTO
{
    public string Key { get; }
    public string PresignUrl { get; }
    public bool IsMain { get; }
}
