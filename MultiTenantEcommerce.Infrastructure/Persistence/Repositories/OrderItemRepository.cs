using MultiTenantEcommerce.Domain.Sales.Entities;
using MultiTenantEcommerce.Domain.Sales.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(AppDbContext appDbContext, TenantContext tenantContext) : base(appDbContext, tenantContext) { }

    public async Task AddBulkAsync(List<OrderItem> items)
    {
         await _appDbContext.OrderItems.AddRangeAsync(items);
    }
}
