using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Enums;
using MultiTenantEcommerce.Domain.Commerce.Catalog.Interfaces;
using MultiTenantEcommerce.Shared.Infrastructure.Messaging;
using MultiTenantEcommerce.Shared.Integration.Events.Logistics;

namespace MultiTenantEcommerce.Application.Commerce.Catalog.Products.EventHandlers.Integration;

public class UpdateStockStatusOnLowStockEventHandler : IAsyncHandler<LowStockIntegrationEvent>
{
    private readonly IProductRepository _productRepository;

    public UpdateStockStatusOnLowStockEventHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task HandleAsync(LowStockIntegrationEvent evt)
    {
        var product = await _productRepository.GetByIdAsync(evt.ProductId);

        if (product == null)
            return;

        product.UpdateStockStatus(StockStatus.LowStock, evt.OccurredOn);
    }
}
