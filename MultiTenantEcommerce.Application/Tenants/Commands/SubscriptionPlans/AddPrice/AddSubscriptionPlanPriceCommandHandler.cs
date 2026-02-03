using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.AddPrice;
public class AddSubscriptionPlanPriceCommandHandler : ICommandHandler<AddSubscriptionPlanPriceCommand, Unit>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddSubscriptionPlanPriceCommandHandler(ISubscriptionPlanRepository subscriptionPlanRepository, 
        IUnitOfWork unitOfWork)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(AddSubscriptionPlanPriceCommand request, CancellationToken cancellationToken)
    {
        var plan = await _subscriptionPlanRepository.GetByStripeProductId(request.StripeProductId)
            ?? throw new Exception("Product not found");

        plan.AddPrice(new Money(request.Amount), request.StripePriceId);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
