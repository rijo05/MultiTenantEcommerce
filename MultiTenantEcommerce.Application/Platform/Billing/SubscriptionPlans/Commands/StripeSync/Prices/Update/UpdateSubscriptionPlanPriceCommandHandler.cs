using MediatR;
using MultiTenantEcommerce.Domain.Platform.Billing.Interfaces;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Commands.StripeSync.Prices.Update;

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