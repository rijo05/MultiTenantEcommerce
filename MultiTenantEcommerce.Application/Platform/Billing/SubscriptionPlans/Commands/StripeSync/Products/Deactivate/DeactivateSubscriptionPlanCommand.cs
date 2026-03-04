using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Commands.StripeSync.Products.Deactivate;

public record DeactivateSubscriptionPlanCommand(
    string StripeProductId) : ICommand<Unit>;