using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Onboarding;
public class CompleteTenantOnboardingCommandHandler : ICommandHandler<CompleteTenantOnboardingCommand, Unit>
{
    public Task<Unit> Handle(CompleteTenantOnboardingCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
