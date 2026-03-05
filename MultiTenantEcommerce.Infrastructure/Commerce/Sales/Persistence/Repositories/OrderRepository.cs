using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Infrastructure.Commerce.Sales.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(SalesDbContext appDbContext) : base(appDbContext) { }

    public async Task<PaginatedList<Order>> GetByCustomerId(
        Guid customerId,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _dbContext.Set<Order>()
            .Include(x => x.Items)
            .Where(x => x.CustomerId == customerId);

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<PaginatedList<Order>> GetFilteredAsync(
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
        var query = _dbContext.Set<Order>()
            .AsNoTracking()
            .Include(o => o.Items)
            .Include(x => x.Payment)
            .AsSplitQuery()
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
            query = query.Where(p => p.TotalPrice.Value >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(p => p.TotalPrice.Value <= maxPrice);

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public override async Task<Order?> GetByIdAsync(Guid orderId)
    {
        return await _dbContext.Set<Order>()
            .Include(x => x.Items)
            .Include(x => x.Payment)
            .FirstOrDefaultAsync(x => x.Id == orderId);
    }

    public async Task<Order?> GetOrderByPaymentReference(string transactionId)
    {
        return await _dbContext.Set<Order>()
            .Include(x => x.Payment)
            .FirstOrDefaultAsync(x => x.Payment != null &&
                                      (x.Payment.TransactionId == transactionId ||
                                       x.Payment.StripeSessionId == transactionId));
    }
}