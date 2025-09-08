using MultiTenantEcommerce.Application.Common.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Context;
public class TenantContext : ITenantContext
{
    public Guid TenantId { get; set; }

    public string? StripeAccountId { get; set; }
}
