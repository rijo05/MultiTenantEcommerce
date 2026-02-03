using MediatR;
using MultiTenantEcommerce.Application.Tenants.Commands.Tenant.RenewSubscription;
using MultiTenantEcommerce.Infrastructure.Webhooks.Interface;
using Stripe;

namespace MultiTenantEcommerce.Infrastructure.Webhooks.Handlers;
public class SubscriptionPaidWebhookHandler : IWebhookHandler
{
    private readonly IMediator _mediator;

    public SubscriptionPaidWebhookHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task HandleAsync(Event stripeEvent)
    {
        var invoice = stripeEvent.Data.Object as Stripe.Invoice;
        if (invoice == null) return;

        string customerId = invoice.CustomerId;

        if (string.IsNullOrEmpty(customerId)) return;

        string subscriptionId = "";
        if (stripeEvent.Data.RawObject != null)
        {
            var parent = stripeEvent.Data.RawObject["parent"];
            if (parent != null && parent["subscription_details"] != null)
            {
                subscriptionId = parent["subscription_details"]?["subscription"]?.ToString();
            }
        }

        if (string.IsNullOrEmpty(subscriptionId)) return;


        await _mediator.Send(new RenewTenantSubscriptionCommand(customerId, subscriptionId, invoice.PeriodEnd));
    }
}
