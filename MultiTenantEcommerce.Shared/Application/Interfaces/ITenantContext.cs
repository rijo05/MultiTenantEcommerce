namespace MultiTenantEcommerce.Shared.Application.Interfaces;

public interface ITenantContext
{
    Guid TenantId { get; }

    void SetTenantId(Guid tenantId);
}