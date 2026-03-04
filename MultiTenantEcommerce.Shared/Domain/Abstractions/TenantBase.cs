using MultiTenantEcommerce.Shared.Utilities.Guards;

namespace MultiTenantEcommerce.Shared.Domain.Abstractions;

public abstract class TenantBase : BaseEntity
{
    protected TenantBase()
    {
    }

    protected TenantBase(Guid tenantId)
    {
        TenantId = tenantId;
    }

    public Guid TenantId { get; private set; }

    public void SetTenantId(Guid tenantId)
    {
        GuardCommon.AgainstEmptyGuid(tenantId, nameof(tenantId));
        TenantId = tenantId;
    }
}