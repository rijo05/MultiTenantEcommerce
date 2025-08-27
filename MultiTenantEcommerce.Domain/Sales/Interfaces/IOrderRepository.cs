using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Sales.Entities;

namespace MultiTenantEcommerce.Domain.Sales.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    public Task<List<OrderItem>> GetItemsByOrderIdAsync(Guid id);
    public Task<List<Order>> GetFilteredAsync(
        Guid? customerId = null,
        string? status = null,
        DateTime? minDate = null,
        DateTime? maxDate = null,
        bool? isPaid = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int page = 1,
        int pageSize = 20,
        SortOptions? sort = null);
}

