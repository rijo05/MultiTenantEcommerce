using MediatR;
using MultiTenantEcommerce.Domain.Platform.Billing.Interfaces;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Commands.StripeSync.Prices.Add;

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