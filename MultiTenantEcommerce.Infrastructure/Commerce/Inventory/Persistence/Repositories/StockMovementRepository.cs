using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Entities;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Interfaces;
using MultiTenantEcommerce.Infrastructure.Commerce.Inventory.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Inventory.Persistence.Repositories;

public class StockMovementRepository : Repository<StockMovement>, IStockMovementRepository
{
    public StockMovementRepository(InventoryDbContext appDbContext) : base(appDbContext)
    {
    }

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
        var query = _dbContext.Set<StockMovement>()
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable();

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