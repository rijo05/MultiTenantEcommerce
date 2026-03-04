using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Commands.StripeSync.Products.Upsert;

public record UpsertSubscriptionPlanCommand(
    string StripeProductId,
    string Name,
    Dictionary<string, string> Metadata) : ICommand<Unit>;