using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Interfaces;

namespace MultiTenantEcommerce.Infrastructure.Repositories;
public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(AppDbContext appDbContext) : base(appDbContext) { }

    public async Task AddBulkAsync(List<OrderItem> items)
    {
         await _appDbContext.OrderItems.AddRangeAsync(items);
    }
}
