using Microsoft.AspNetCore.Mvc;
using MultiTenantEcommerce.Infrastructure.Webhooks.Interface;
using Stripe;

namespace MultiTenantEcommerce.API.Controllers.Webhook;

[ApiController]
[Route("api/[controller]")]
public class WebhooksController : ControllerBase
{
    private readonly string _secret;
    private readonly IWebhookDispatcher _dispatcher;

    public WebhooksController(IConfiguration config, IWebhookDispatcher dispatcher)
    {
        _secret = config["Stripe:WebhookSecret"];
        _dispatcher = dispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _secret
            );

            await _dispatcher.ProcessAsync(stripeEvent);

            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest();
        }
    }
}
