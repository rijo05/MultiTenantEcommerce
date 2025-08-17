using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Infrastructure.Context;
using System.Data;

namespace MultiTenantEcommerce.Infrastructure.Repositories;
public class StockMovementRepository : Repository<StockMovement>, IStockMovementRepository
{
    public StockMovementRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task<List<StockMovement>> GetFilteredAsync(
    Guid? productId = null, 
    int? quantity = null, 
    DateTime? minDate = null, 
    DateTime? maxDate = null, 
    string? reason = null, 
    int page = 1, 
    int pageSize = 20, 
    SortOptions? sort = null)
    {
        var query = _appDbContext.StockMovements.AsQueryable();

        if (productId.HasValue)
            query = query.Where(p => p.ProductId == productId);

        if (quantity.HasValue)
            query = query.Where(p => p.Quantity == quantity);

        if (minDate.HasValue)
            query = query.Where(p => p.CreatedAt >= minDate);

        if (maxDate.HasValue)
            query = query.Where(p => p.CreatedAt <= maxDate);

        if (!string.IsNullOrWhiteSpace(reason))
            query = query.Where(p => EF.Functions.Like(p.Reason.ToString(), $"%{reason}%"));


        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}
