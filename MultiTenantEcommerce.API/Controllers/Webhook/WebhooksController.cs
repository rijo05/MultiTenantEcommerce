using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Commands.Completed;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Commands.Failed;
using Stripe;

namespace MultiTenantEcommerce.API.Controllers.Webhook;

[ApiController]
[Route("api/[controller]")]
public class WebhooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public WebhooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("stripe")]
    public async Task<IActionResult> StripeWebhook()
    {
        const string endpointSecret = "whsec_...";

        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        Event stripeEvent = EventUtility.ParseEvent(json);
        var signatureHeader = Request.Headers["Stripe-Signature"];

        stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, endpointSecret);


        if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            var paymentId = Guid.Parse(paymentIntent.Metadata["PaymentId"]);

            var payment = new MarkOrderPaymentAsCompletedCommand(paymentId, paymentIntent.Id);

            await _mediator.Send(payment);
        }
        else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            var paymentId = Guid.Parse(paymentIntent.Metadata["PaymentId"]);

            var payment = new MarkPaymentAsFailedCommand(paymentId, paymentIntent.LastPaymentError.Message);

            await _mediator.Send(payment);
        }
        else
        {
            //loggar talvez ou simplesmente ignorar
        }

        return NoContent();
    }

    [HttpPost("paypal")]
    public async Task<IActionResult> PaypalWebhook()
    {

        throw new NotImplementedException();
    }
}
