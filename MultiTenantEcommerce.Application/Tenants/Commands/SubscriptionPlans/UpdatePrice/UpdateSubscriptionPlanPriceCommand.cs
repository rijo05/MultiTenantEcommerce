using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.UpdatePrice;
public record UpdateSubscriptionPlanPriceCommand(
    string StripeProductId,
    string StripePriceId,
    bool IsActive) : ICommand<Unit>;
    