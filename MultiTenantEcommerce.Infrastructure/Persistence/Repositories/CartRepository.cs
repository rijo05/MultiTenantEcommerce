using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(AppDbContext appDbContext, TenantContext tenantContext)
        : base(appDbContext, tenantContext) { }

    public async Task<Cart?> GetByCustomerIdAsync(Guid customerId)
    {
        return await _appDbContext.Carts
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.IsOpen && x.CustomerId == customerId);
    }
}
