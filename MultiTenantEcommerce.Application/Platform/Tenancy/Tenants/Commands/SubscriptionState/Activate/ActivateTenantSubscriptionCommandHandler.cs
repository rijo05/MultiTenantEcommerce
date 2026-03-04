using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.SubscriptionState.Activate;

public class ActivateTenantSubscriptionCommandHandler : ICommandHandler<ActivateTenantSubscriptionCommand, Unit>
{
    public Task<Unit> Handle(ActivateTenantSubscriptionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}