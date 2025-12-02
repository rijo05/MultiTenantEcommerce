using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Payment.Entities;

namespace MultiTenantEcommerce.Domain.Payment.Interfaces;
public interface IOrderPaymentRepository : IRepository<OrderPayment>
{
    public Task<List<OrderPayment>> GetByCustomerId(
        Guid customerId,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);
    public Task<OrderPayment?> GetByOrderId(Guid orderId);
    public Task<List<OrderPayment>> GetFilteredAsync(
        Guid? customerId = null,
        PaymentStatus? status = null,
        PaymentMethod? method = null,
        DateTime? minCreatedAt = null,
        DateTime? maxCreatedAt = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);
}
