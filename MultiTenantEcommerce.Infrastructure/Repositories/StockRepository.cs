using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Infrastructure.Context;

namespace MultiTenantEcommerce.Infrastructure.Repositories;
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

    public async Task<List<Stock>> GetBulkByIdsAsync(List<Guid> ids)
    {
        return await _appDbContext.Stocks.Where(x => ids.Contains(x.ProductId)).ToListAsync();
    }

    public async Task SaveAsync(Stock stock)
    {
        // Verifica se o RowVersion foi alterado
        _appDbContext.Entry(stock).OriginalValues["RowVersion"] = stock.RowVersion;

        // Salva as alterações no banco de dados
        await _appDbContext.SaveChangesAsync();
    }
}
