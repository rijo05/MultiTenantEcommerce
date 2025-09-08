using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext) { }

    public async Task<Order?> GetByIdWithItemsAsync(Guid orderId)
    {
        return await _appDbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<List<Order>> GetByCustomerIdWithItems(Guid customerId)
    {
        return await _appDbContext.Orders
            .Include(x => x.Items)
            .Where(x => x.CustomerId == customerId).ToListAsync();
    }

    public async Task<List<Order>> GetFilteredAsync(
        Guid? customerId = null,
        string? status = null,
        DateTime? minDate = null,
        DateTime? maxDate = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.Orders
            .Include(o => o.Items)
            .AsQueryable();

        if (customerId.HasValue)
            query = query.Where(p => p.CustomerId == customerId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(p => EF.Functions.Like(p.OrderStatus.ToString(), $"%{status}%"));

        if (minDate.HasValue)
            query = query.Where(p => p.CreatedAt >= minDate);

        if (maxDate.HasValue)
            query = query.Where(p => p.CreatedAt <= maxDate);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price.Value >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price.Value <= maxPrice);

        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}
