using MediatR;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.HandleWebhook;
using MultiTenantEcommerce.Infrastructure.Webhooks.Interface;
using Stripe;
using Stripe.Checkout;

namespace MultiTenantEcommerce.Infrastructure.Webhooks.Handlers;

public class StripeCheckoutExpiredHandler : IWebhookHandler
{
    private readonly IMediator _mediator;

    public StripeCheckoutExpiredHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task HandleAsync(Event stripeEvent)
    {
        if (stripeEvent.Data.Object is not Session session)
            return;

        if (session.Metadata.TryGetValue("OrderId", out var orderIdStr) && session.Metadata.TryGetValue("PaymentId", out var paymentIdStr))
        {
            if (Guid.TryParse(orderIdStr, out var orderId) && Guid.TryParse(paymentIdStr, out var paymentId))
            {
                await _mediator.Send(new MarkOrderAsFailedCommand(orderId, paymentId, "timeout"));
            }
        }
    }
}