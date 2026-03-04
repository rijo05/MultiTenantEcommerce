using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Integration.Proxies;
public interface IIdentityIntegrationProxy
{
    public Task<Guid?> GetUserIdByEmailAsync(string email);
}
