namespace MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
public interface ITenantContext
{
    Guid TenantId { get; }

    void SetTenantId(Guid tenantId);
}
