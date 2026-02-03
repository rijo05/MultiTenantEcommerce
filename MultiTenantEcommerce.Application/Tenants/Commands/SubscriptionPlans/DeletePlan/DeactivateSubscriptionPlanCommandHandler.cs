using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.DeletePlan;
public class DeactivateSubscriptionPlanCommandHandler : ICommandHandler<DeactivateSubscriptionPlanCommand, Unit>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateSubscriptionPlanCommandHandler(ISubscriptionPlanRepository subscriptionPlanRepository, 
        IUnitOfWork unitOfWork)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeactivateSubscriptionPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await _subscriptionPlanRepository.GetByStripeProductId(request.StripeProductId)
            ?? throw new Exception("Product not found");

        plan.Deactivate();

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
