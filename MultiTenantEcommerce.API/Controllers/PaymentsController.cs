using MediatR;
using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Application.Payment.Commands.StripeCompleted;
using MultiTenantEcommerce.Application.Payment.Commands.StripeFailed;
using MultiTenantEcommerce.Application.Payment.DTOs;
using MultiTenantEcommerce.Application.Payment.Queries.GetById;
using MultiTenantEcommerce.Application.Payment.Queries.GetFiltered;
using Stripe;

namespace MultiTenantEcommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<ActionResult<List<PaymentResponseDTO>>> GetPayments([FromQuery] GetFilteredPaymentsQuery paymentsQuery)
    {
        var payments = await _mediator.Send(paymentsQuery);

        return Ok(payments);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PaymentResponseDTO>> GetById(Guid id)
    {
        var query = new GetPaymentByIdQuery(id);

        var payment = await _mediator.Send(query);

        return payment;
    }

    [HttpPost("webhook/stripe")]
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

            var payment = new MarkPaymentAsCompletedCommand(paymentId, paymentIntent.Id);

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

    [HttpPost("webhook/paypal")]
    public async Task<IActionResult> PaypalWebhook()
    {

        throw new NotImplementedException();
    }
}
