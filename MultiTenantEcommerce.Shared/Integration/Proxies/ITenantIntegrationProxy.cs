using MultiTenantEcommerce.Shared.Integration.DTOs;

namespace MultiTenantEcommerce.Shared.Integration.Proxies;
public interface ITenantIntegrationProxy
{
    Task<TenantProfileDTO> GetTenantProfileAsync(Guid tenantId);

    Task<decimal> GetTenantPlanLimitsAsync(Guid tenantId);
}
