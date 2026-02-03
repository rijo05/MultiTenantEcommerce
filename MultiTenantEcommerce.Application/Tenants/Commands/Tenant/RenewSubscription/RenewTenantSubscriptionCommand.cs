using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.RenewSubscription;
public record RenewTenantSubscriptionCommand(
    string StripeCustomerId,
    string SubscriptionId,
    DateTime PeriodEnd) : ICommand<Unit>;
