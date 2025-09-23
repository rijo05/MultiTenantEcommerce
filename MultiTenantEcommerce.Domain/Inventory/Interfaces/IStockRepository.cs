using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Inventory.Entities;

namespace MultiTenantEcommerce.Domain.Inventory.Interfaces;
public interface IStockRepository : IRepository<Stock>
{
    public Task<Stock?> GetByProductIdAsync(Guid productId);
    public Task AddBulkAsync(List<Stock> items);
    public Task<List<Stock>> GetBulkByIdsAsync(IEnumerable<Guid> ids);
    public void UpdateWithRow(Stock stock);
}
