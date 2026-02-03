using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;

namespace MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.UpdatePrice;
public class UpdateSubscriptionPlanPriceCommandHandler : ICommandHandler<UpdateSubscriptionPlanPriceCommand, Unit>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSubscriptionPlanPriceCommandHandler(ISubscriptionPlanRepository subscriptionPlanRepository, 
        IUnitOfWork unitOfWork)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateSubscriptionPlanPriceCommand request, CancellationToken cancellationToken)
    {
        var plan = await _subscriptionPlanRepository.GetByStripeProductId(request.StripeProductId)
            ?? throw new Exception("Product not found");

        plan.UpdatePriceStatus(request.StripePriceId, request.IsActive);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
