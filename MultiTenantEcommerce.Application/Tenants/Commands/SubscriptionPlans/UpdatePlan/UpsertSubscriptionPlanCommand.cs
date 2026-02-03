using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.UpdatePlan;
public record UpsertSubscriptionPlanCommand(
    string StripeProductId,
    string Name,
    Dictionary<string, string> Metadata) : ICommand<Unit>;
