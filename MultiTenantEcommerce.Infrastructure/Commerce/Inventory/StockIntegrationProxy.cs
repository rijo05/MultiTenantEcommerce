using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;
using MultiTenantEcommerce.Shared.Integration.DTOs;
using MultiTenantEcommerce.Shared.Integration.Proxies;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Inventory;
public class StockIntegrationProxy : ILogisticsIntegrationProxy
{
    private readonly IStockRepository _stockRepository;
    private readonly IStockService _stockService;

    public StockIntegrationProxy(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public Task<decimal> CalculateQuoteAsync(string carrier, string address)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<StockProxyDTO>> GetStocksByProductIdsAsync(IEnumerable<Guid> productIds)
    {
        var stocks = await _stockRepository.GetBulkByProductIdsAsync(productIds);

        return stocks.Select(x => new StockProxyDTO(x.ProductId, x.StockAvailableAtMoment.Value));
    }

    public Task<bool> TryReserveStockAsync(IEnumerable<(Guid ProductId, int Quantity)> items)
    {
        return _stockService.TryReserveStock(items.ToList());
    }
}
