using MediatR;
using MultiTenantEcommerce.Application.Tenants.Commands.Tenant.Onboarding;
using MultiTenantEcommerce.Infrastructure.Webhooks.Interface;
using Stripe;

namespace MultiTenantEcommerce.Infrastructure.Webhooks.Handlers;
public class AccountUpdatedWebhookHandler : IWebhookHandler
{
    private readonly IMediator _mediator;

    public AccountUpdatedWebhookHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task HandleAsync(Event stripeEvent)
    {
        var account = stripeEvent.Data.Object as Account;

        //onboard para receber dinheiro
        if (account.ChargesEnabled && account.DetailsSubmitted)
        {
            await _mediator.Send(new CompleteTenantOnboardingCommand(
                StripeAccountId: account.Id
            ));
        }
    }
}
