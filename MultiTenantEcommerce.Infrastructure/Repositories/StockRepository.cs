using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Infrastructure.Context;

namespace MultiTenantEcommerce.Infrastructure.Repositories;
public class StockRepository : Repository<Stock>, IStockRepository
{
    public StockRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<Stock?> GetByProductId(Guid productId)
    {
        return await _appDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public async Task AddBulkAsync(List<Stock> items)
    {
        await _appDbContext.Stocks.AddRangeAsync(items);
    }
}
