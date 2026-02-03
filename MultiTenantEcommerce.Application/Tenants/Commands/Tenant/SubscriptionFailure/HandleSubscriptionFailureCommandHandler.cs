using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.SubscriptionFailure;
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
