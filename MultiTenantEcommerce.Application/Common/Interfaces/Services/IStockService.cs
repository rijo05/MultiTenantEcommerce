using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Services;
public interface IStockService
{
    public Task<bool> CheckAvailability(Guid productId, PositiveQuantity quantity);
    public Task<bool> TryReserveStockWithRetries(List<CartItem> items, int retries = 3);
    public Task CommitStock(Order order);
    public Task ReleaseReservedStock(Order order);
}
