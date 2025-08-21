using MultiTenantEcommerce.Domain.Entities;

namespace MultiTenantEcommerce.Domain.Interfaces;
public interface IStockRepository : IRepository<Stock>
{
    public Task<Stock?> GetByProductIdAsync(Guid productId);
    public Task AddBulkAsync(List<Stock> items);
    public Task<List<Stock>> GetBulkByIdsAsync(List<Guid> ids);
    public Task SaveAsync(Stock stock);
}
