using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Catalog.Products.DTOs.Images;
public interface IProductImageDTO
{
    string Key { get; }
    string PresignUrl { get; }
    bool IsMain { get; }
}
