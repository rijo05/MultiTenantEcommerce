using MultiTenantEcommerce.Domain.Commerce.Inventory.Events;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.EventHandlers;

public class StockMovementEventHandler : IEventHandler<StockMovementEvent>
{
    private readonly IStockMovementRepository _stockMovementRepository;

    public StockMovementEventHandler(IStockMovementRepository stockMovementRepository)
    {
        _stockMovementRepository = stockMovementRepository;
    }

    public async Task HandleAsync(StockMovementEvent domainEvent)
    {
        var movement = new StockMovement(domainEvent.TenantId,
            domainEvent.ProductId,
            domainEvent.Quantity,
            domainEvent.StockMovementReason);

        await _stockMovementRepository.AddAsync(movement);
    }
}