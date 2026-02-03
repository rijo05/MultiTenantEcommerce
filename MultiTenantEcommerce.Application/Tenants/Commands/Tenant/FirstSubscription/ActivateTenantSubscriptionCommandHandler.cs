using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.FirstSubscription;
public class ActivateTenantSubscriptionCommandHandler : ICommandHandler<ActivateTenantSubscriptionCommand, Unit>
{
    public Task<Unit> Handle(ActivateTenantSubscriptionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
