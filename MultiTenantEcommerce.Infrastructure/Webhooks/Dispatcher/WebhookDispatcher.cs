using Microsoft.Extensions.DependencyInjection;
using MultiTenantEcommerce.Infrastructure.Payments;
using MultiTenantEcommerce.Infrastructure.Webhooks.Handlers;
using MultiTenantEcommerce.Infrastructure.Webhooks.Interface;
using Stripe;

namespace MultiTenantEcommerce.Infrastructure.Webhooks.Dispatcher;
public class WebhookDispatcher : IWebhookDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public WebhookDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ProcessAsync(Event stripeEvent)
    {
        using var scope = _serviceProvider.CreateScope();
        IWebhookHandler? handler = null;

        switch (stripeEvent.Type)
        {
            case StripeEvents.CheckoutSessionCompleted:
                handler = scope.ServiceProvider.GetRequiredService<StripeCheckoutCompletedHandler>();
                break;

            case StripeEvents.CheckoutSessionExpired:
                handler = scope.ServiceProvider.GetRequiredService<StripeCheckoutExpiredHandler>();
                break;

            case StripeEvents.InvoicePaid:
                handler = scope.ServiceProvider.GetRequiredService<SubscriptionPaidWebhookHandler>();
                break;

            case StripeEvents.InvoicePaymentFailed:
                handler = scope.ServiceProvider.GetRequiredService<SubscriptionFailedWebhookHandler>();
                break;

            case StripeEvents.AccountUpdated:
                handler = scope.ServiceProvider.GetRequiredService<AccountUpdatedWebhookHandler>();
                break;

            case StripeEvents.ProductCreated:
            case StripeEvents.ProductUpdated:
            case StripeEvents.PriceCreated:
            case StripeEvents.PriceUpdated:
            case StripeEvents.PriceDeleted:
                handler = scope.ServiceProvider.GetRequiredService<StripeProductHandler>();
                break;
        }

        if (handler != null)
        {
            await handler.HandleAsync(stripeEvent);
        }
    }
}
