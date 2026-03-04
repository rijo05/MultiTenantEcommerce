using MediatR;
using MultiTenantEcommerce.Domain.Platform.Billing.Interfaces;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Commands.StripeSync.Products.Deactivate;

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