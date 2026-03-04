using MultiTenantEcommerce.Domain.Commerce.Inventory.Entities;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;

public interface IStockRepository : IRepository<Stock>
{
    public Task<Stock?> GetByProductIdAsync(Guid productId);
    public Task AddBulkAsync(List<Stock> items);
    public Task<List<Stock>> GetBulkByProductIdsAsync(IEnumerable<Guid> ids);
    public Task<int> ReserveStockAsync(Guid productId, int quantity);
}