using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Payment.Entities;
using MultiTenantEcommerce.Domain.Payment.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class OrderPaymentRepository : Repository<OrderPayment>, IOrderPaymentRepository
{
    public OrderPaymentRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<List<OrderPayment>> GetByCustomerId(
        Guid customerId,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.OrderPayments.Where(x => x.CustomerId == customerId);

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

    public async Task<OrderPayment?> GetByOrderId(Guid orderId)
    {
        return await _appDbContext.OrderPayments.FirstOrDefaultAsync(x => x.OrderId == orderId);
    }

    public async Task<List<OrderPayment>> GetFilteredAsync(
        Guid? customerId = null,
        PaymentStatus? status = null,
        PaymentMethod? method = null,
        DateTime? minCreatedAt = null,
        DateTime? maxCreatedAt = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.OrderPayments
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable();

        if (customerId.HasValue)
            query = query.Where(p => p.CustomerId == customerId.Value);

        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        if (method.HasValue)
            query = query.Where(p => p.PaymentMethod == method.Value);

        if (minCreatedAt.HasValue)
            query = query.Where(p => p.CreatedAt >= minCreatedAt.Value);

        if (maxCreatedAt.HasValue)
            query = query.Where(p => p.CreatedAt <= maxCreatedAt.Value);

        return await SortAndPageAsync(query, sort, page, pageSize);
    }

}
