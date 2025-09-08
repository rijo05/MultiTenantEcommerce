namespace MultiTenantEcommerce.Application.Common.Interfaces;
public interface ITenantContext
{
    Guid TenantId { get; }
    string? StripeAccountId { get; }
}
