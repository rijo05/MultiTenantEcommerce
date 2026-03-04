using MultiTenantEcommerce.Domain.Commerce.Inventory.Events;
using MultiTenantEcommerce.Shared.Application.Events;
using MultiTenantEcommerce.Shared.Infrastructure.Messaging;
using MultiTenantEcommerce.Shared.Integration.Events.Logistics;


namespace MultiTenantEcommerce.Application.Commerce.Inventory.EventHandlers;
public class StockLevelChangedEventHandler : ISyncHandler<StockLevelChangedEvent>
{
    private readonly IIntegrationEventPublisher _eventPublisher;

    public StockLevelChangedEventHandler(IIntegrationEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    public Task HandleAsync(StockLevelChangedEvent evt)
    {
        if (evt.AvailableAfter <= 0)
        {
            _eventPublisher.AddEvent(new OutOfStockIntegrationEvent(evt.TenantId, evt.ProductId));
        }
        else if (evt.AvailableAfter <= evt.MinAfter)
        {
            _eventPublisher.AddEvent(new LowStockIntegrationEvent(evt.TenantId, evt.ProductId, evt.AvailableAfter, evt.MinAfter));
        }
        else
            _eventPublisher.AddEvent(new InStockIntegrationEvent(evt.TenantId, evt.ProductId));

        
        return Task.CompletedTask;
    }
}
