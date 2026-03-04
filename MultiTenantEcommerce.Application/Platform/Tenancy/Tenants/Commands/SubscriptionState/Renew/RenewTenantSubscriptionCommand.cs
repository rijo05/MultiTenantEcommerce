using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.SubscriptionState.Renew;

public record RenewTenantSubscriptionCommand(
    string StripeCustomerId,
    string SubscriptionId,
    DateTime PeriodEnd) : ICommand<Unit>;