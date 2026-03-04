using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Shared.Integration.DTOs;
using MultiTenantEcommerce.Shared.Integration.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Infrastructure.IntegrationProxies;
public class TenantIntegrationProxy : ITenantIntegrationProxy
{
    private readonly ITenantRepository _tenantRepository;

    public TenantIntegrationProxy(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<TenantProfileDTO> GetTenantProfileAsync(Guid tenantId)
    {
       var tenant = await _tenantRepository.GetByIdAsync(tenantId);

        return new TenantProfileDTO(tenant.Name, tenant.SubDomain, tenant.Email.Value);
    }
}
