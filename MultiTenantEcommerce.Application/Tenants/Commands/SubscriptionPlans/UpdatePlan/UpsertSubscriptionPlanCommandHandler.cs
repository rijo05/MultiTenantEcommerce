using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using System.Security.AccessControl;

namespace MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.UpdatePlan;
public class UpsertSubscriptionPlanCommandHandler : ICommandHandler<UpsertSubscriptionPlanCommand, Unit>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

    public UpsertSubscriptionPlanCommandHandler(ISubscriptionPlanRepository subscriptionPlanRepository)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
    }

    public Task<Unit> Handle(UpsertSubscriptionPlanCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
