using MultiTenantEcommerce.Domain.Commerce.Catalog.Events;
using MultiTenantEcommerce.Shared.Application.Events;
using MultiTenantEcommerce.Shared.Infrastructure.Messaging;
using MultiTenantEcommerce.Shared.Integration.Events.Commerce;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.EventHandlers.Domain;
public class ProductImageDeletedEventHandler : ISyncHandler<ProductImageDeletedEvent>
{
    private readonly IIntegrationEventPublisher _publisher;

    public ProductImageDeletedEventHandler(IIntegrationEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task HandleAsync(ProductImageDeletedEvent evt)
    {
        _publisher.AddEvent(new ProductImageDeletedIntegrationEvent(
            evt.TenantId,
            evt.ProductId,
            evt.ImageId,
            evt.ImageKey));

        await Task.CompletedTask;
    }
}
