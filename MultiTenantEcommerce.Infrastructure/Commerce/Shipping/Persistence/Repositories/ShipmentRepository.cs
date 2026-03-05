using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Entities;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Interfaces;
using MultiTenantEcommerce.Infrastructure.Commerce.Shipping.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Shipping.Persistence.Repositories;

public class ShipmentRepository : Repository<Shipment>, IShipmentRepository
{
    public ShipmentRepository(ShippingDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Shipment?> GetByOrderId(Guid orderId)
    {
        return await _dbContext.Set<Shipment>().FirstOrDefaultAsync(x => x.OrderId == orderId);
    }
}