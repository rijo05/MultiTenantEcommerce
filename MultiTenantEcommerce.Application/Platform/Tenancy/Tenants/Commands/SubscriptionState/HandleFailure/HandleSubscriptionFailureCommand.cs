using MediatR;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.SubscriptionState.HandleFailure;

public record HandleSubscriptionFailureCommand(string StripeCustomerId) : ICommand<Unit>;