using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Infrastructure.Context;

namespace MultiTenantEcommerce.Infrastructure.Repositories;
public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext) { }

    public async Task AddBulkAsync(List<OrderItem> items)
    {
         await _appDbContext.OrderItems.AddRangeAsync(items);
    }
}
