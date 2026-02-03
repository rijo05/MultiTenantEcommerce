using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Shipping.Enums;
using MultiTenantEcommerce.Domain.Tenants.Events;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Tenants.Entities;
public class Tenant : BaseEntity
{
    public string Name { get; private set; }
    public Email Email { get; private set; }

    public string? StripeAccountId { get; private set; } //receive money
    public string? StripeCustomerId { get; private set; } //pay subscription
    public TenantSubscription Subscription { get; private set; }

    private readonly List<ShippingProviderConfig> _shippingProviders = new();
    public IReadOnlyCollection<ShippingProviderConfig> ShippingProviders => _shippingProviders.AsReadOnly();


    private Tenant() { }
    public Tenant(string companyName, Email companyEmail, SubscriptionPlan plan)
    {
        GuardCommon.AgainstNullOrEmpty(companyName, nameof(companyName));
        GuardCommon.AgainstMaxLength(companyName, 50, nameof(companyName));

        Name = companyName;
        Email = companyEmail;

        var activePrice = plan.Prices.Single(x => x.IsActive);
        Subscription = new TenantSubscription(plan, activePrice);

        AddDomainEvent(new TenantRegisteredEvent(this.Id, Email.Value));
    }

    #region SUBSCRIPTION

    public void SetStripeConnectId(string accountId)
    {
        StripeAccountId = accountId;
    }

    public void SetStripeCustomerId(string customerId)
    {
        StripeCustomerId = customerId;
    }

    public void RenewSubscription(string subcriptionId, DateTime endDate)
        => Subscription.ActivateOrRenew(subcriptionId, endDate);

    public void HandlePaymentFailure()
        => Subscription.MarkAsPastDue();

    public void ChangePlan(SubscriptionPlan newPlan)
        => Subscription.SwitchPlan(newPlan);

    #endregion

    #region UPDATE

    public void UpdateTenant(string companyName)
    {
        if (!string.IsNullOrWhiteSpace(companyName))
            UpdateCompanyName(companyName);
    }

    public void UpdateCompanyName(string companyName)
    {
        GuardCommon.AgainstNullOrEmpty(companyName, nameof(companyName));
        GuardCommon.AgainstMaxLength(companyName, 50, nameof(companyName));

        Name = companyName;
        SetUpdatedAt();
    }

    #endregion

    #region SHIPPING

    public void ActivateCarrier(ShipmentCarrier carrier)
    {
        var config = GetCarrierConfig(carrier);
        config.Activate();
    }

    public void DeactivateCarrier(ShipmentCarrier carrier)
    {
        var config = GetCarrierConfig(carrier);
        config.Deactivate();
    }

    private ShippingProviderConfig GetCarrierConfig(ShipmentCarrier carrier)
        => _shippingProviders.FirstOrDefault(x => x.Carrier == carrier)
           ?? throw new Exception("Carrier not found");

    #endregion
}
