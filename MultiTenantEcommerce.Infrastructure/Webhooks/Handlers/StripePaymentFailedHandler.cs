using MediatR;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.HandleWebhook;
using MultiTenantEcommerce.Infrastructure.Webhooks.Interface;
using Stripe;

namespace MultiTenantEcommerce.Infrastructure.Webhooks.Handlers;
public class StripePaymentFailedHandler : IWebhookHandler
{
    private readonly IMediator _mediator;

    public StripePaymentFailedHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task HandleAsync(Event stripeEvent)
    {
        if (stripeEvent.Data.Object is not PaymentIntent paymentIntent)
            return;

        if (paymentIntent.Metadata.TryGetValue("OrderId", out var orderIdStr) && paymentIntent.Metadata.TryGetValue("PaymentId", out var paymentIdStr))
        {
            if (Guid.TryParse(orderIdStr, out var orderId) && Guid.TryParse(paymentIdStr, out var paymentId))
            {
                var failureMessage = paymentIntent.LastPaymentError?.Message
                                     ?? "No reason given";

                await _mediator.Send(new MarkOrderAsFailedCommand(orderId, paymentId, failureMessage));
            }
        }
    }
}
