using MultiTenantEcommerce.Domain.Entities;

namespace MultiTenantEcommerce.Domain.Interfaces;
public interface IStockRepository : IRepository<Stock>
{
    public Task<Stock?> GetByProductId(Guid productId);
    public Task AddBulkAsync(List<Stock> items);
}
