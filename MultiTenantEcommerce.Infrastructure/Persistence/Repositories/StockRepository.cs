using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class StockRepository : Repository<Stock>, IStockRepository
{
    public StockRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<Stock?> GetByProductIdAsync(Guid productId)
    {
        return await _appDbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public async Task AddBulkAsync(List<Stock> items)
    {
        await _appDbContext.Stocks.AddRangeAsync(items);
    }

    public async Task<List<Stock>> GetBulkByProductIdsAsync(IEnumerable<Guid> ids)
    {
        return await _appDbContext.Stocks.Where(x => ids.Contains(x.ProductId)).ToListAsync();
    }

    public async Task<int> DecreaseStockAsync(Guid productId, int quantity)
    {
        var rowsAffected = await _appDbContext.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""Stocks""
            SET ""Quantity"" = ""Quantity"" - {quantity},
                ""UpdatedAt"" = {DateTime.UtcNow}
            WHERE ""ProductId"" = {productId} 
                AND ""Quantity"" >= {quantity}
        ");

        return rowsAffected;
    }
}
