using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Domain.Tenants.Entities;
public class SubscriptionPlanPrice : BaseEntity
{
    public Guid PlanId { get; private set; }
    public SubscriptionPlan Plan { get; private set; }

    public Money Price { get; private set; }

    public string StripePriceId { get; private set; }

    public bool IsActive { get; private set; }

    internal SubscriptionPlanPrice(SubscriptionPlan plan, Money price, string stripePriceId)
    {
        PlanId = plan.Id;
        Plan = plan;
        Price = price;
        StripePriceId = stripePriceId;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    internal void Deactivate() => IsActive = false;
    internal void Activate() => IsActive = true;
}
