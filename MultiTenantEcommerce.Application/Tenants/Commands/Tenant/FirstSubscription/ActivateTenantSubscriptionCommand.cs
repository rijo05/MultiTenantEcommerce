using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.FirstSubscription;
public record ActivateTenantSubscriptionCommand(
    Guid TenantId,
    string StripeCustomerId,
    string StripeSubscriptionId) : ICommand<Unit>;
