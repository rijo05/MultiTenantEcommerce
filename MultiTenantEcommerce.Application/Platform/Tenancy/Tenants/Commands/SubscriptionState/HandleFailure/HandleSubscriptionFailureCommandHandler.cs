using MediatR;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.SubscriptionState.HandleFailure;

public class HandleSubscriptionFailureCommandHandler : ICommandHandler<HandleSubscriptionFailureCommand, Unit>
{
    private readonly ITenantRepository _tenantRepository;

    public HandleSubscriptionFailureCommandHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<Unit> Handle(HandleSubscriptionFailureCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByStripeCustomerId(request.StripeCustomerId)
                     ?? throw new Exception("Tenant not found");

        tenant.HandlePaymentFailure();

        return Unit.Value;
    }
}