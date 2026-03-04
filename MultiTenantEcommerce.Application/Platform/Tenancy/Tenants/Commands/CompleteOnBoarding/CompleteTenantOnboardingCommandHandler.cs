using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.CompleteOnBoarding;

public class CompleteTenantOnboardingCommandHandler : ICommandHandler<CompleteTenantOnboardingCommand, Unit>
{
    public Task<Unit> Handle(CompleteTenantOnboardingCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}