using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Tenants.Entities;

namespace MultiTenantEcommerce.Domain.Tenants.Interfaces;
public interface ISubscriptionPlanRepository : IRepository<SubscriptionPlan>
{
    public Task<SubscriptionPlan?> GetByStripeProductId(string stripeProductId);
}
