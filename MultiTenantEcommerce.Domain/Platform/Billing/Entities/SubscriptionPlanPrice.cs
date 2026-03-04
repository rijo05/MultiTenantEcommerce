using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Platform.Billing.Entities;

public class SubscriptionPlanPrice : BaseEntity
{
    internal SubscriptionPlanPrice(SubscriptionPlan plan, Money price, string stripePriceId)
    {
        PlanId = plan.Id;
        Plan = plan;
        Price = price;
        StripePriceId = stripePriceId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid PlanId { get; private set; }
    public SubscriptionPlan Plan { get; private set; }

    public Money Price { get; private set; }

    public string StripePriceId { get; private set; }

    public bool IsActive { get; private set; }

    internal void Deactivate()
    {
        IsActive = false;
    }

    internal void Activate()
    {
        IsActive = true;
    }
}