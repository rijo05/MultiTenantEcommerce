using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.Common.Entities;
public abstract class TenantBase : BaseEntity
{
    public Guid TenantId { get; private set; }

    protected TenantBase() { }
    protected TenantBase(Guid tenantId)
    {
        TenantId = tenantId;
    }

    public void SetTenantId(Guid tenantId)
    {
        GuardCommon.AgainstEmptyGuid(tenantId, nameof(tenantId));
        TenantId = tenantId;
    }
}
