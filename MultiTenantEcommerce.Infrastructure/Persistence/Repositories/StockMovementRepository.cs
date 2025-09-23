using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Domain.Inventory.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using System.Data;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class StockMovementRepository : Repository<StockMovement>, IStockMovementRepository
{
    public StockMovementRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext) { }

    public async Task<List<StockMovement>> GetFilteredAsync(
    Guid? productId = null,
    int? minQuantity = null,
    int? maxQuantity = null,
    string? reason = null,
    DateTime? minDate = null,
    DateTime? maxDate = null,
    int page = 1,
    int pageSize = 20,
    SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.StockMovements.AsQueryable();

        if (productId.HasValue)
            query = query.Where(p => p.ProductId == productId);

        if (minQuantity.HasValue)
            query = query.Where(p => p.Quantity >= minQuantity);

        if (maxQuantity.HasValue)
            query = query.Where(p => p.Quantity <= maxQuantity);

        if (minDate.HasValue)
            query = query.Where(p => p.CreatedAt >= minDate);

        if (maxDate.HasValue)
            query = query.Where(p => p.CreatedAt <= maxDate);

        if (!string.IsNullOrWhiteSpace(reason))
            query = query.Where(p => EF.Functions.Like(p.Reason.ToString(), $"%{reason}%"));


        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}
