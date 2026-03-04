using MultiTenantEcommerce.Domain.Platform.Tenancy.Enums;
using MultiTenantEcommerce.Shared.Domain.Abstractions;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;

public class TenantSubscription : BaseEntity
{
    public Guid PlanId { get; private set; }
    public Guid PlanPriceId { get; private set; }

    public string? StripeSubscriptionId { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateTime CurrentPeriodStart { get; private set; }
    public DateTime CurrentPeriodEnd { get; private set; }
    public DateTime? PastDueDate { get; private set; }
    public bool CancelAtPeriodEnd { get; private set; }

    const int TRIAL_DAYS = 14;

    private TenantSubscription()
    {
    }

    internal TenantSubscription(Guid planId, Guid planPriceId)
    {
        PlanId = planId;
        PlanPriceId = planPriceId;

        Status = SubscriptionStatus.Trial;
        CurrentPeriodStart = DateTime.UtcNow;
        CurrentPeriodEnd = DateTime.UtcNow.AddDays(TRIAL_DAYS);

        CancelAtPeriodEnd = false;
    }

    public void ActivateOrRenew(string stripeSubscriptionId, DateTime periodEnd)
    {
        StripeSubscriptionId = stripeSubscriptionId;
        Status = SubscriptionStatus.Active;
        CurrentPeriodEnd = periodEnd;
        CurrentPeriodStart = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void MarkAsPastDue()
    {
        Status = SubscriptionStatus.PastDue;
        PastDueDate = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void CancelAtPeriodEndRequest()
    {
        CancelAtPeriodEnd = true;
        SetUpdatedAt();
    }

    public void CancelNow()
    {
        Status = SubscriptionStatus.Canceled;
        CancelAtPeriodEnd = false;
        SetUpdatedAt();
    }

    internal void SwitchPlan(Guid newPlanId, Guid newPlanPriceId)
    {
        PlanId = newPlanId;
        PlanPriceId = newPlanPriceId;
        SetUpdatedAt();
    }

    public int GetDaysLeftInTrial()
    {
        if (Status != SubscriptionStatus.Trial) return 0;

        var daysLeft = (CurrentPeriodEnd - DateTime.UtcNow).Days;
        return daysLeft > 0 ? daysLeft : 0;
    }

    public bool HasTrialExpired()
    {
        return Status == SubscriptionStatus.Trial && DateTime.UtcNow > CurrentPeriodEnd;
    }

    public void SuspendIfTrialExpired()
    {
        if (HasTrialExpired())
        {
            Status = SubscriptionStatus.Suspended;
            SetUpdatedAt();
        }
    }
}