using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    public Task<PaginatedList<Order>> GetByCustomerId(
        Guid customerId,
        int page = 1,
        int pageSize = 20,
        SortOptions sort = SortOptions.TimeDesc);

    public Task<Order?> GetOrderByPaymentReference(string TransactionId);

    public Task<PaginatedList<Order>> GetFilteredAsync(
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