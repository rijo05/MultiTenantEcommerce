using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.RenewSubscription;
public class RenewTenantSubscriptionCommandHandler : ICommandHandler<RenewTenantSubscriptionCommand, Unit>
{
    private readonly ITenantRepository _tenantRepository;

    public RenewTenantSubscriptionCommandHandler(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task<Unit> Handle(RenewTenantSubscriptionCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByStripeCustomerId(request.StripeCustomerId) 
            ?? throw new Exception("Tenant not found");

        tenant.RenewSubscription(request.SubscriptionId, request.PeriodEnd);

        return Unit.Value;
    }
}
