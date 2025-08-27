using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Entities;

namespace MultiTenantEcommerce.Domain.Sales.Interfaces;
public interface IOrderItemRepository : IRepository<OrderItem>
{
    public Task AddBulkAsync(List<OrderItem> items);
}
