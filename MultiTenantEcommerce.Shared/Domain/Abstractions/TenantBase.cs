using MultiTenantEcommerce.Shared.Domain.Utilities.Guards;

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

        if (TenantId != Guid.Empty && TenantId != tenantId)
            throw new InvalidOperationException("Cannot reassign an entity to a different Tenant.");

        TenantId = tenantId;
    }
}