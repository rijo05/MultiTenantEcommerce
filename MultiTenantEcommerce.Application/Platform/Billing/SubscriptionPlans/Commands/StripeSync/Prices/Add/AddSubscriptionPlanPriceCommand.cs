using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Commands.StripeSync.Prices.Add;

public record AddSubscriptionPlanPriceCommand(
    string StripeProductId,
    string StripePriceId,
    long Amount) : ICommand<Unit>;