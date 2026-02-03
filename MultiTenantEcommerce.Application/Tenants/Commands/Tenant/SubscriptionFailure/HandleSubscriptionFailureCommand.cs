using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.SubscriptionFailure;
public record HandleSubscriptionFailureCommand(string StripeCustomerId) : ICommand<Unit>;
