using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Payment.Interfaces;
public interface IPaymentRepository : IRepository<Entities.Payment>
{
    public Task<Entities.Payment?> GetByOrderId(Guid orderId);
    public Task<List<Entities.Payment>> GetFilteredAsync(
        Guid? payerId = null,
        Guid? payeeId = null,
        PaymentStatus? status = null,
        DateTime? minDate = null,
        DateTime? maxDate = null,
        PaymentReason? reason = null,
        Guid? reasonId = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);
}
