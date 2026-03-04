using MultiTenantEcommerce.Domain.Platform.Billing.Entities;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Platform.Billing.Interfaces;

public interface ISubscriptionPlanRepository : IRepository<SubscriptionPlan>
{
    public Task<SubscriptionPlan?> GetByStripeProductId(string stripeProductId);
}