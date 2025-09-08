using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;

namespace MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    public Task<Order?> GetByIdWithItemsAsync(Guid orderId);

    public Task<List<Order>> GetByCustomerIdWithItems(Guid customerId);

    public Task<List<Order>> GetFilteredAsync(
        Guid? customerId = null,
        string? status = null,
        DateTime? minDate = null,
        DateTime? maxDate = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);
}

