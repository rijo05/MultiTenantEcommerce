using MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;
using MultiTenantEcommerce.Shared.Infrastructure.Messaging;
using MultiTenantEcommerce.Shared.Integration.Events.Commerce;


namespace MultiTenantEcommerce.Application.Commerce.Inventory.EventHandlers;
public class RevertReservedStockOnOrderCheckoutFailEventHandler : IAsyncHandler<OrderCheckoutFailIntegrationEvent>
{
    private readonly IStockRepository _stockRepository;

    public RevertReservedStockOnOrderCheckoutFailEventHandler(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task HandleAsync(OrderCheckoutFailIntegrationEvent evt)
    {
        var stocks = await _stockRepository.GetBulkByProductIdsAsync(evt.items.Select(x => x.ProductId));

        foreach (var item in evt.items)
        {
            var stock = stocks.FirstOrDefault(s => s.ProductId == item.ProductId);
            if (stock != null)
                stock.ReleaseReservedStock(item.Quantity);
        }
    }
}
