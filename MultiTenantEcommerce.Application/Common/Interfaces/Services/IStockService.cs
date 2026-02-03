using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Common.Interfaces.Services;
public interface IStockService
{
    public Task<bool> CheckAvailability(Guid productId, PositiveQuantity quantity);
    public Task<bool> TryReserveStock(List<(Guid ProductId, int Quantity)> items);
    public Task CommitStock(Order order);
    public Task ReleaseReservedStock(Order order);
}
