using MultiTenantEcommerce.Domain.Commerce.Catalog.Events;
using MultiTenantEcommerce.Shared.Application.Events;
using MultiTenantEcommerce.Shared.Infrastructure.Messaging;
using MultiTenantEcommerce.Shared.Integration.Events.Commerce;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.EventHandlers.Domain;
public class ProductCreatedEventHandler : ISyncHandler<ProductCreatedEvent>
{
    private readonly IntegrationEventPublisher _publisher;

    public ProductCreatedEventHandler(IntegrationEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task HandleAsync(ProductCreatedEvent evt)
    {
        _publisher.AddEvent(new ProductCreatedIntegrationEvent(evt.TenantId, evt.ProductId));

        await Task.CompletedTask;
    }
}
