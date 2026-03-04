using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Platform.Billing.Entities;

public class SubscriptionPlan : BaseEntity
{
    private readonly List<SubscriptionPlanPrice> _prices = new();

    private SubscriptionPlan()
    {
    }

    public SubscriptionPlan(string name, string stripeProductId, PlanLimits planLimits)
    {
        Name = name;
        StripeProductId = stripeProductId;
        PlanLimits = planLimits;
        IsActive = true;
    }

    public string Name { get; private set; }
    public string StripeProductId { get; private set; }
    public PlanLimits PlanLimits { get; private set; }
    public bool IsActive { get; private set; }
    public IReadOnlyCollection<SubscriptionPlanPrice> Prices => _prices.AsReadOnly();

    public void AddPrice(Money price, string stripePriceId)
    {
        if (_prices.Any(x => x.StripePriceId == stripePriceId)) return;

        _prices.Add(new SubscriptionPlanPrice(this, price, stripePriceId));

        foreach (var p in _prices) p.Deactivate();
    }

    public void Deactivate()
    {
        IsActive = false;

        foreach (var price in _prices)
            price.Deactivate();
    }

    public void UpdatePriceStatus(string stripePriceId, bool isActive)
    {
        var price = _prices.FirstOrDefault(x => x.StripePriceId == stripePriceId);
        if (price == null) return;

        if (isActive)
        {
            foreach (var p in _prices) p.Deactivate();
            price.Activate();
        }
        else
        {
            price.Deactivate();
        }
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Update(string name, PlanLimits newLimits)
    {
        Name = name;
        PlanLimits = newLimits;
    }
}