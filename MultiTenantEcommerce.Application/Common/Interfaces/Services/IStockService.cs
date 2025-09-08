using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Services;
public interface IStockService
{
    public Task<bool> CheckAvailability(Guid productId, PositiveQuantity quantity);
    Task<bool> TryReserveStockWithRetries(List<CartItem> items, int retries = 3);
}
