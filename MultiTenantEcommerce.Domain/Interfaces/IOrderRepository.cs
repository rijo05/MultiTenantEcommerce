using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    public Task<List<OrderItem>> GetItemsByOrderIdAsync(Guid id);
    public Task<List<Order>> GetFilteredAsync(
        Guid? tenantId = null,
        Guid? customerId = null,
        OrderStatus? status = null,
        DateTime? minDate = null,
        DateTime? maxDate = null,
        bool? isPaid = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int page = 1,
        int pageSize = 20,
        string? sort = null);
}

