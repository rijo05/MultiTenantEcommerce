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

    public async Task<bool> TryReserveStockWithRetries(List<CartItem> items, int retries = 3)
    {
        var pendingItems = new List<CartItem>(items);

        for (int attempt = 0; attempt < retries; attempt++)
        {
            var failedItems = new List<CartItem>();

            var stocks = await _stockRepository.GetBulkByIdsAsync(pendingItems.Select(x => x.Product.Id).ToList());
            var stockDict = stocks.ToDictionary(x => x.ProductId);

            foreach (var item in pendingItems)
            {
                if (!stockDict.TryGetValue(item.Product.Id, out var stock))
                    throw new Exception($"Garantia extra NUNCA deve entrar aqui");

                bool reserved = false;

                for (int prodAttempt = 0; prodAttempt < retries; prodAttempt++)
                {
                    try
                    {
                        stock.ReserveStock(item.Quantity.Value);
                        _stockRepository.UpdateWithRow(stock);
                        reserved = true;
                        break;
                    }
                    catch (ConcurrencyException)
                    {
                        if (prodAttempt < retries - 1)
                            await Task.Delay(200);
                    }
                    catch (/*InsufficientStockException*/ Exception)
                    {
                        reserved = false;
                        break;
                    }
                }

                if (!reserved)
                    failedItems.Add(item);
            }

            if (!failedItems.Any())
                return true;

            pendingItems = failedItems;

            if (attempt < retries - 1)
                await Task.Delay(500);
        }

        return false;
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
        var stocks = await _stockRepository.GetBulkByIdsAsync(order.Items.Select(x => x.ProductId).ToList());

        var stockDict = stocks.ToDictionary(s => s.ProductId);

        foreach (var item in order.Items)
        {
            if (!stockDict.TryGetValue(item.ProductId, out var stock))
                throw new Exception($"Stock not found for ProductId {item.ProductId}");

            action(stock, item.Quantity.Value);
        }
    }

}
