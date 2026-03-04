using MultiTenantEcommerce.Domain.Commerce.Catalog.Enums;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Infrastructure.Messaging;
using MultiTenantEcommerce.Shared.Integration.Events.Logistics;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.EventHandlers.Integration;
public class UpdateStockStatusOnOutOfStockEventHandler : IAsyncHandler<OutOfStockIntegrationEvent>
{
    private readonly IProductRepository _productRepository;

    public UpdateStockStatusOnOutOfStockEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task HandleAsync(OutOfStockIntegrationEvent evt)
    {
        var product = await _productRepository.GetByIdAsync(evt.ProductId);

        if (product == null)
            return;

        product.UpdateStockStatus(StockStatus.OutOfStock, evt.OccurredOn);
    }
}
