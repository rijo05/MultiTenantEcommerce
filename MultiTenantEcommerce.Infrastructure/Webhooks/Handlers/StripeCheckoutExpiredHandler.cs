using MediatR;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Commands.Failed;
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
        var session = stripeEvent.Data.Object as Session;

        if (session.Metadata.TryGetValue("OrderId", out var orderIdStr))
        {
            var orderId = Guid.Parse(orderIdStr);

            await _mediator.Send(new MarkOrderAsFailedCommand(orderId));
        }
    }
}
