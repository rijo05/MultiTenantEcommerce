using MediatR;
using MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.AddPrice;
using MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.DeletePlan;
using MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.DeletePrice;
using MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.UpdatePlan;
using MultiTenantEcommerce.Application.Tenants.Commands.SubscriptionPlans.UpdatePrice;
using MultiTenantEcommerce.Infrastructure.Payments;
using MultiTenantEcommerce.Infrastructure.Webhooks.Interface;
using Stripe;

namespace MultiTenantEcommerce.Infrastructure.Webhooks.Handlers;
public class StripeProductHandler : IWebhookHandler
{
    private readonly IMediator _mediator;

    public StripeProductHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task HandleAsync(Event stripeEvent)
    {
        if (stripeEvent.Data.Object is Product product)
        {
            if (stripeEvent.Type == StripeEvents.ProductDeleted)
            {
                await _mediator.Send(new DeactivateSubscriptionPlanCommand(
                    StripeProductId: product.Id));
            }
            else // created | updated
            {
                await _mediator.Send(new UpsertSubscriptionPlanCommand(
                    StripeProductId: product.Id,
                    Name: product.Name,
                    Metadata: product.Metadata));
            }

            return;
        }

        if (stripeEvent.Data.Object is Price price)
        {
            if (stripeEvent.Type == StripeEvents.PriceDeleted || !price.Active)
            {
                await _mediator.Send(new UpdateSubscriptionPlanPriceCommand(
                    StripeProductId: price.ProductId,
                    StripePriceId: price.Id,
                    IsActive: false));
            }
            else if (stripeEvent.Type == StripeEvents.PriceCreated)
            {
                if (price.Recurring?.Interval != "month") return;

                await _mediator.Send(new AddSubscriptionPlanPriceCommand(
                    StripeProductId: price.ProductId,
                    StripePriceId: price.Id,
                    Amount: price.UnitAmount ?? 0));
            }
            else if (stripeEvent.Type == StripeEvents.PriceUpdated)
            {
                await _mediator.Send(new UpdateSubscriptionPlanPriceCommand(
                   StripeProductId: price.ProductId,
                   StripePriceId: price.Id,
                   IsActive: price.Active));
                //metadata
            }

            return;
        }
    }
}
