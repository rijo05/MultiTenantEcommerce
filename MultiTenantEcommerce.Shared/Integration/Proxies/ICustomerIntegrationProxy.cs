using MultiTenantEcommerce.Shared.Integration.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.Proxies;
public interface ICustomerIntegrationProxy
{
    Task<Dictionary<Guid, string>> GetCustomerNamesByIdsAsync(IEnumerable<Guid> customerIds);
    Task<CustomerInfoDTO> GetCustomerInfoByIdAsync(Guid customerId);
}
