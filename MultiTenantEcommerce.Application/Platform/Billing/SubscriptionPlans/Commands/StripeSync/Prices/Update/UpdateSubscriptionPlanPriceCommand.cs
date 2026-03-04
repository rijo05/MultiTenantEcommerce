using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Commands.StripeSync.Prices.Update;

public record UpdateSubscriptionPlanPriceCommand(
    string StripeProductId,
    string StripePriceId,
    bool IsActive) : ICommand<Unit>;