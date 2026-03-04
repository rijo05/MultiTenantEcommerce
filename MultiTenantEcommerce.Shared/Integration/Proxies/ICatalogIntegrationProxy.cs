using MultiTenantEcommerce.Shared.Integration.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.Proxies;
public interface ICatalogIntegrationProxy
{
    Task<List<ProductInfoDTO>> GetProductsByIds(IEnumerable<Guid> ProductIds);
    Task<ProductInfoDTO> GetProductById(Guid ProductId);
}
