using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Enums;
using MultiTenantEcommerce.Infrastructure.Commerce.Sales.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

//public class OrderPaymentRepository : Repository<OrderPayment>, IOrderPaymentRepository
//{
//    public OrderPaymentRepository(SalesDbContext appDbContext) : base(appDbContext)
//    {
//    }

//    public async Task<List<OrderPayment>> GetByCustomerId(
//        Guid customerId,
//        int page = 1,
//        int pageSize = 20,
//        SortOptions sort = SortOptions.TimeDesc)
//    {
//        var query = _dbContext.OrderPayments.Where(x => x.CustomerId == customerId);

//        return await SortAndPageAsync(query, sort, page, pageSize);
//    }

//    public async Task<IEnumerable<OrderPayment>> GetByOrderId(Guid orderId)
//    {
//        return await _dbContext.OrderPayments
//            .AsNoTracking()
//            .Where(x => x.OrderId == orderId)
//            .OrderByDescending(x => x.CreatedAt)
//            .ToListAsync();
//    }

//    public async Task<List<OrderPayment>> GetFilteredAsync(
//        Guid? customerId = null,
//        PaymentStatus? status = null,
//        PaymentMethod? method = null,
//        DateTime? minCreatedAt = null,
//        DateTime? maxCreatedAt = null,
//        int page = 1,
//        int pageSize = 20,
//        SortOptions sort = SortOptions.TimeDesc)
//    {
//        var query = _dbContext.OrderPayments
//            .AsNoTracking()
//            .AsSplitQuery()
//            .AsQueryable();

//        if (customerId.HasValue)
//            query = query.Where(p => p.CustomerId == customerId.Value);

//        if (status.HasValue)
//            query = query.Where(p => p.Status == status.Value);

//        if (method.HasValue)
//            query = query.Where(p => p.PaymentMethod == method.Value);

//        if (minCreatedAt.HasValue)
//            query = query.Where(p => p.CreatedAt >= minCreatedAt.Value);

//        if (maxCreatedAt.HasValue)
//            query = query.Where(p => p.CreatedAt <= maxCreatedAt.Value);

//        return await SortAndPageAsync(query, sort, page, pageSize);
//    }
//}