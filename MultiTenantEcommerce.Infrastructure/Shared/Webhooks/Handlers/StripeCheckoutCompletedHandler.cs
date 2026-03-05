using MediatR;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.HandleWebhook;
using MultiTenantEcommerce.Application.Tenancy.Tenants.Commands.SubscriptionState.Activate;
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
        if (stripeEvent.Data.Object is not Session session || session == null)
            return;

        if (session.Mode == "payment" && 
            session.Metadata.TryGetValue("OrderId", out var orderIdStr) && 
            session.Metadata.TryGetValue("PaymentId", out var paymentIdStr))
        {
            if (Guid.TryParse(orderIdStr, out var orderId) && Guid.TryParse(paymentIdStr, out var paymentId))
            {
                await _mediator.Send(new MarkOrderAsPaidCommand(orderId, paymentId, session.PaymentIntentId));
            }
        }
        //apenas a primeira subscricao
        else if (session.Mode == "subscription")
        {
            var customerId = session.CustomerId;

            if (session.Metadata.TryGetValue("TenantId", out var tenantIdStr))
            {
                if (Guid.TryParse(tenantIdStr, out var tenantId))
                {
                    await _mediator.Send(new ActivateTenantSubscriptionCommand(
                        TenantId: tenantId,
                        StripeCustomerId: customerId,
                        StripeSubscriptionId: session.SubscriptionId));
                }
            }
        }
    }
}