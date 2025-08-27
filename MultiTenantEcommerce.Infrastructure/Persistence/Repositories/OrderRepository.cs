using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Sales.Entities;
using MultiTenantEcommerce.Domain.Sales.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext) { }

    public async Task<List<Order>> GetFilteredAsync(
        Guid? customerId = null,
        string? status = null,
        DateTime? minDate = null,
        DateTime? maxDate = null,
        bool? isPaid = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int page = 1,
        int pageSize = 20,
        SortOptions? sort = null)
    {
        var query = _appDbContext.Orders.AsQueryable();

        if(customerId.HasValue)
            query = query.Where(p => p.CustomerId == customerId.Value);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(p => EF.Functions.Like(p.OrderStatus.ToString(), $"%{status}%"));

        if (minDate.HasValue)
            query = query.Where(p => p.CreatedAt >= minDate);

        if (maxDate.HasValue)
            query = query.Where(p => p.CreatedAt <= maxDate);

        if (isPaid.HasValue)
            query = query.Where(p => p.PayedAt.HasValue != isPaid);

        if (minPrice.HasValue)
            query = query.Where(p => p.Price.Value >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price.Value <= maxPrice);

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<List<OrderItem>> GetItemsByOrderIdAsync(Guid id)
    {
        return await _appDbContext.Set<OrderItem>().Where(x => x.OrderId == id).ToListAsync();
    }
}
