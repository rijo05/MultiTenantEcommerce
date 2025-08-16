using MultiTenantEcommerce.Domain.Entities;

namespace MultiTenantEcommerce.Domain.Interfaces;
public interface IOrderItemRepository : IRepository<OrderItem>
{
    public Task AddBulkAsync(List<OrderItem> items);
}
