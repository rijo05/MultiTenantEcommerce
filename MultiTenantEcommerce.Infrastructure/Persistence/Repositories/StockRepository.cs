using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class StockRepository : Repository<Stock>, IStockRepository
{
    public StockRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext) { }

    public async Task<Stock?> GetByProductIdAsync(Guid productId)
    {
        return await _appDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public async Task AddBulkAsync(List<Stock> items)
    {
        await _appDbContext.Stocks.AddRangeAsync(items);
    }

    public async Task<List<Stock>> GetBulkByIdsAsync(IEnumerable<Guid> ids)
    {
        return await _appDbContext.Stocks.Where(x => ids.Contains(x.ProductId)).ToListAsync();
    }

    public void UpdateWithRow(Stock stock)
    {
        _appDbContext.Entry(stock).OriginalValues["RowVersion"] = stock.RowVersion;
        _appDbContext.Stocks.Update(stock);
    }
}
