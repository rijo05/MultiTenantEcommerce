using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Tenants.Entities;
public class TenantSubscription : BaseEntity
{
    public Guid PlanId { get; private set; }
    public SubscriptionPlan Plan { get; private set; }

    public Guid PlanPriceId { get; private set; }
    public SubscriptionPlanPrice PlanPrice { get; private set; }

    public string? StripeSubscriptionId { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateTime CurrentPeriodStart { get; private set; }
    public DateTime CurrentPeriodEnd { get; private set; }
    public DateTime? PastDueDate { get; private set; }
    public bool CancelAtPeriodEnd { get; private set; }

    private TenantSubscription() { }

    internal TenantSubscription(SubscriptionPlan plan, SubscriptionPlanPrice planPrice)
    {
        Plan = plan;
        PlanId = plan.Id;

        PlanPrice = planPrice;
        PlanPriceId = planPrice.Id;


        Status = SubscriptionStatus.Incomplete;
        CurrentPeriodStart = DateTime.UtcNow;
        CurrentPeriodEnd = DateTime.UtcNow;

        CancelAtPeriodEnd = false;
    }

    public void ActivateOrRenew(string stripeSubscriptionId, DateTime periodEnd)
    {
        StripeSubscriptionId = stripeSubscriptionId;
        Status = SubscriptionStatus.Active;
        CurrentPeriodEnd = periodEnd;
        CurrentPeriodStart = DateTime.UtcNow;
    }

    public void MarkAsPastDue()
    {
        Status = SubscriptionStatus.PastDue;
        PastDueDate = DateTime.UtcNow;
    }

    public void CancelAtPeriodEndRequest()
    {
        CancelAtPeriodEnd = true;
    }

    public void CancelNow()
    {
        Status = SubscriptionStatus.Canceled;
        CancelAtPeriodEnd = false;
    }

    internal void SwitchPlan(SubscriptionPlan newPlan)
    {
        Plan = newPlan;
        PlanId = newPlan.Id;
    }
}
