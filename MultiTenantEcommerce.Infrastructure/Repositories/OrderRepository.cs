using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Infrastructure.Context;

namespace MultiTenantEcommerce.Infrastructure.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext appDbContext) : base(appDbContext) { }

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
            query = query.Where(p => p.Price >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice);

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<List<OrderItem>> GetItemsByOrderIdAsync(Guid id)
    {
        return await _appDbContext.Set<OrderItem>().Where(x => x.OrderId == id).ToListAsync();
    }
}
