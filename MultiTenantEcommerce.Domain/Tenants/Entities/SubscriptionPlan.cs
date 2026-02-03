using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;
using System.Reflection.Metadata.Ecma335;

namespace MultiTenantEcommerce.Domain.Tenants.Entities;
public class SubscriptionPlan : BaseEntity
{
    public string Name { get; private set; }
    public string StripeProductId { get; private set; }
    public PlanLimits PlanLimits { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<SubscriptionPlanPrice> _prices = new();
    public IReadOnlyCollection<SubscriptionPlanPrice> Prices => _prices.AsReadOnly();

    private SubscriptionPlan() { }
    public SubscriptionPlan(string name, string stripeProductId, PlanLimits planLimits)
    {
        Name = name;
        StripeProductId = stripeProductId;
        PlanLimits = planLimits;
        IsActive = true;
    }

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

    public void Activate() => IsActive = true;
}
