using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Entities;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;
using MultiTenantEcommerce.Infrastructure.Commerce.Inventory.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Inventory.Persistence.Repositories;

public class StockRepository : Repository<Stock>, IStockRepository
{
    public StockRepository(InventoryDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Stock?> GetByProductIdAsync(Guid productId)
    {
        return await _dbContext.Set<Stock>().FirstOrDefaultAsync(x => x.ProductId == productId);
    }

    public async Task AddBulkAsync(List<Stock> items)
    {
        await _dbContext.Set<Stock>().AddRangeAsync(items);
    }

    public async Task<List<Stock>> GetBulkByProductIdsAsync(IEnumerable<Guid> ids)
    {
        return await _dbContext.Set<Stock>().Where(x => ids.Contains(x.ProductId)).ToListAsync();
    }

    public async Task<int> ReserveStockAsync(Guid productId, int quantity)
    {
        var rowsAffected = await _dbContext.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE ""Stocks""
            SET ""Reserved"" = ""Reserved"" + {{quantity}},
                ""UpdatedAt"" = {{DateTime.UtcNow}}
            WHERE ""ProductId"" = {{productId}}
                AND (""Quantity"" - ""Reserved"") >= {{quantity}}
        ");

        return rowsAffected;
    }
}