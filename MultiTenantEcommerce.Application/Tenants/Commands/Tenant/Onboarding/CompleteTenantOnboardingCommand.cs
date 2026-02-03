using MediatR;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;

namespace MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Onboarding;
public record CompleteTenantOnboardingCommand(
    string StripeAccountId) : ICommand<Unit>;
