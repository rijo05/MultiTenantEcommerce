using MultiTenantEcommerce.Domain.Platform.Tenancy.Events;
using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.Utilities.Guards;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; private set; }
    public string SubDomain { get; private set; }
    public CustomDomain? CustomDomain { get; private set; }
    public Email? Email { get; private set; }

    public string? StripeAccountId { get; private set; } //receive money
    public string? StripeCustomerId { get; private set; } //pay subscription
    public TenantSubscription Subscription { get; }

    private Tenant()
    {
    }
    public Tenant(string companyName, Guid subscriptionPlanId, Guid subscriptionPriceId, string subDomain)
    {
        GuardCommon.AgainstNullOrEmpty(companyName, nameof(companyName));
        GuardCommon.AgainstMaxLength(companyName, 50, nameof(companyName));

        Name = companyName;

        SubDomain = $"{subDomain.ToLower().Replace(" ", "-")}.plataforma.com";

        Subscription = new TenantSubscription(subscriptionPlanId, subscriptionPriceId);

        AddDomainEvent(new TenantRegisteredEvent(Id));
    }

    #region SUBSCRIPTION

    public void SetStripeAccountId(string stripeAccountId)
    {
        if (string.IsNullOrWhiteSpace(stripeAccountId))
            throw new Exception("Stripe account id cannot be empty.");

        StripeAccountId = stripeAccountId;
    }

    public void SetStripeCustomerId(string stripeCustomerId)
    {
        if (string.IsNullOrWhiteSpace(stripeCustomerId))
            throw new Exception("Stripe customer id cannot be empty.");

        StripeCustomerId = stripeCustomerId;
    }

    public void RenewSubscription(string subcriptionId, DateTime endDate)
    {
        Subscription.ActivateOrRenew(subcriptionId, endDate);
    }

    public void HandlePaymentFailure()
    {
        Subscription.MarkAsPastDue();
    }

    public void ChangePlan(Guid newPlanId, Guid newPriceId)
    {
        Subscription.SwitchPlan(newPlanId, newPriceId);
        SetUpdatedAt();
    }

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

    public void SetCompanyEmail(Email newEmail)
    {
        Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
        SetUpdatedAt();
    }

    #endregion

    public void SetCustomDomain(string domain)
    {
        GuardCommon.AgainstNullOrEmpty(domain, nameof(domain));

        CustomDomain = new CustomDomain(Id, domain);
        SetUpdatedAt();
    }
}