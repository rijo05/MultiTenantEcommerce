using Stripe;

namespace MultiTenantEcommerce.Infrastructure.Webhooks.Interface;
public interface IWebhookHandler
{
    public Task HandleAsync(Event stripeEvent);
}
