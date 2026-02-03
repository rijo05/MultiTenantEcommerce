using Stripe;

namespace MultiTenantEcommerce.Infrastructure.Webhooks.Interface;
public interface IWebhookDispatcher
{
    public Task ProcessAsync(Event stripeEvent);
}
