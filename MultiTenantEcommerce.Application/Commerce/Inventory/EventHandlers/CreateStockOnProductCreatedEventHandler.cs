using MultiTenantEcommerce.Domain.Commerce.Inventory.Entities;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;
using MultiTenantEcommerce.Domain.Logistics.Inventory.Entities;
using MultiTenantEcommerce.Shared.Infrastructure.Messaging;
using MultiTenantEcommerce.Shared.Integration.Events.Commerce;

namespace MultiTenantEcommerce.Application.Commerce.Inventory.EventHandlers;
public class CreateStockOnProductCreatedEventHandler : IAsyncHandler<ProductCreatedIntegrationEvent>
{
    private readonly IStockRepository _stockRepository;

    public CreateStockOnProductCreatedEventHandler(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task HandleAsync(ProductCreatedIntegrationEvent evt)
    {
        var stock = await _stockRepository.GetByProductIdAsync(evt.ProductId);

        if (stock == null)
        {
            stock = new Stock(evt.TenantId, evt.ProductId, 0, 0);

            await _stockRepository.AddAsync(stock);
        }
    }
}
