using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.Entities;
public abstract class TenantBase
{
    public Guid TenantId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected TenantBase() { }
    protected TenantBase(Guid tenantId) 
    {
        TenantId = tenantId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;   
    }

    public void SetCreatedAt()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public void SetUpdatedAt()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetTenantId(Guid tenantId)
    {
        GuardCommon.AgainstEmptyGuid(tenantId, nameof(tenantId));
        TenantId = tenantId;
    }
}
