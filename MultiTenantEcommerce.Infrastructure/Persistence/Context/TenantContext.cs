using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Context;
public class TenantContext : ITenantContext
{
    public Guid TenantId { get; set; }

    public void SetTenantId(Guid tenantId)
    {
        TenantId = tenantId;
    }
}
