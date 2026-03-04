using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.SubscriptionState.Activate;

public record ActivateTenantSubscriptionCommand(
    Guid TenantId,
    string StripeCustomerId,
    string StripeSubscriptionId) : ICommand<Unit>;