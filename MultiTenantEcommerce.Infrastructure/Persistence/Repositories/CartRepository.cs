using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(AppDbContext appDbContext)
        : base(appDbContext)
    {
    }

    public async Task<Cart?> GetByCustomerIdAsync(Guid customerId)
    {
        return await _appDbContext.Carts
            .Include(x => x.Items)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.IsOpen && x.CustomerId == customerId);
    }

    public async Task<Cart?> GetActiveCartAsync(Guid? customerId, Guid? anonymousId)
    {
        if (customerId == null && anonymousId == null) return null;

        var query = _appDbContext.Carts
            .Include(x => x.Items)
            .Where(x => x.IsOpen);

        if (customerId.HasValue)
            query = query.Where(x => x.CustomerId == customerId);
        else if (anonymousId.HasValue)
            query = query.Where(x => x.AnonymousId == anonymousId);

        return await query.FirstOrDefaultAsync();
    }
}