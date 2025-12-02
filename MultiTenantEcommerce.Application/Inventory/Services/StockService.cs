using MultiTenantEcommerce.Application.Common.Exceptions;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Inventory.Services;
public class StockService : IStockService
{
    private readonly IStockRepository _stockRepository;

    public StockService(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<bool> CheckAvailability(Guid productId, PositiveQuantity quantity)
    {
        var stock = await _stockRepository.GetByProductIdAsync(productId)
            ?? throw new Exception("Product doesnt exist.");

        return stock.CheckAvailability(quantity.Value);
    }

    public async Task<bool> TryReserveStock(List<(Guid ProductId, int Quantity)> items)
    {
        var sortedItems = items.OrderBy(x => x.ProductId).ToList();

        foreach (var item in sortedItems)
        {
            var rows = await _stockRepository.DecreaseStockAsync(item.ProductId, item.Quantity);

            if (rows == 0)
            {
                //Falta de stock
                return false;
            }
        }
        return true;
    }

    public async Task CommitStock(Order order)
    {
        await ApplyStockAction(order, (stock, qty) => stock.CommitStock(qty));
    }

    public async Task ReleaseReservedStock(Order order)
    {
        await ApplyStockAction(order, (stock, qty) => stock.ReleaseReservedStock(qty));
    }

    private async Task ApplyStockAction(Order order, Action<Stock, int> action)
    {
        var stocks = await _stockRepository.GetBulkByProductIdsAsync(order.Items.Select(x => x.ProductId).ToList());

        var stockDict = stocks.ToDictionary(s => s.ProductId);

        foreach (var item in order.Items)
        {
            if (!stockDict.TryGetValue(item.ProductId, out var stock))
                throw new Exception($"Stock not found for ProductId {item.ProductId}");

            action(stock, item.Quantity.Value);
        }
    }

}
