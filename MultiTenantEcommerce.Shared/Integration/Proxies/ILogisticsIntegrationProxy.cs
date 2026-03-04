using MultiTenantEcommerce.Shared.Integration.DTOs;

namespace MultiTenantEcommerce.Shared.Integration.Proxies;
public interface ILogisticsIntegrationProxy
{
    Task<IEnumerable<StockProxyDTO>> GetStocksByProductIdsAsync(IEnumerable<Guid> productIds);

    Task<decimal> CalculateQuoteAsync(string carrier, string address);

    public Task<bool> TryReserveStockAsync(IEnumerable<(Guid ProductId, int Quantity)> items);
}
