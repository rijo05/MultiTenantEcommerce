using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Payment.Entities;
using MultiTenantEcommerce.Domain.Payment.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext)
    {
    }

    public async Task<Payment?> GetByOrderId(Guid orderId)
    {
        return await _appDbContext.Payments
            .FirstOrDefaultAsync(x => x.ReasonId == orderId &&
                                x.Reason == PaymentReason.Order);
    }

    public async Task<List<Payment>> GetFilteredAsync(
        Guid? payerId = null,
        Guid? payeeId = null,
        PaymentStatus? status = null,
        DateTime? minDate = null,
        DateTime? maxDate = null,
        PaymentReason? reason = null,
        Guid? reasonId = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc)
    {
        var query = _appDbContext.Payments.AsQueryable();

        if (payerId.HasValue)
            query = query.Where(p => p.PayerId == payerId.Value);

        if (payeeId.HasValue)
            query = query.Where(p => p.PayeeId == payeeId.Value);

        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        if (minDate.HasValue)
            query = query.Where(p => p.CreatedAt >= minDate.Value);

        if (maxDate.HasValue)
            query = query.Where(p => p.CreatedAt <= maxDate.Value);

        if (reason.HasValue)
            query = query.Where(p => p.Reason == reason.Value);

        if (reasonId.HasValue)
            query = query.Where(p => p.ReasonId == reasonId.Value);


        return await SortAndPageAsync(query, sort, page, pageSize);
    }
}
