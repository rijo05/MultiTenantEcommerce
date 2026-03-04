namespace MultiTenantEcommerce.Shared.Infrastructure.Persistence;

public interface ITenantContext
{
    Guid TenantId { get; }

    void SetTenantId(Guid tenantId);
}