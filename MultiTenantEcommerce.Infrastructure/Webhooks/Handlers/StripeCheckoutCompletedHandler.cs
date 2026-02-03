using MediatR;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Commands.Completed;
using MultiTenantEcommerce.Application.Tenants.Commands.Tenant.FirstSubscription;
using MultiTenantEcommerce.Infrastructure.Webhooks.Interface;
using Stripe;
using Stripe.Checkout;

namespace MultiTenantEcommerce.Infrastructure.Webhooks.Handlers;
public class StripeCheckoutCompletedHandler : IWebhookHandler
{
    private readonly IMediator _mediator;

    public StripeCheckoutCompletedHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task HandleAsync(Event stripeEvent)
    {
        var session = stripeEvent.Data.Object as Session;

        if (session.Mode == "payment" && session.Metadata.TryGetValue("OrderId", out var orderIdStr))
        {
            var orderId = Guid.Parse(orderIdStr);

            await _mediator.Send(new MarkOrderAsPaidCommand(orderId, session.PaymentIntentId));
        }
        //apenas a primeira subscricao
        else if (session.Mode == "subscription")
        {
            var customerId = session.CustomerId;

            if (session.Metadata.TryGetValue("TenantId", out var tenantIdStr))
            {
                await _mediator.Send(new ActivateTenantSubscriptionCommand(
                    TenantId: Guid.Parse(tenantIdStr),
                    StripeCustomerId: customerId,
                    StripeSubscriptionId: session.SubscriptionId
                ));
            }
        }
    }
}
