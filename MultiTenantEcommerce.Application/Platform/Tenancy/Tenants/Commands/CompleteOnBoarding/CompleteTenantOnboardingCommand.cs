using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.CompleteOnBoarding;

public record CompleteTenantOnboardingCommand(
    string StripeAccountId) : ICommand<Unit>;