using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Domain.Shipping.Entities;
using MultiTenantEcommerce.Domain.Shipping.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class ShipmentRepository : Repository<Shipment>, IShipmentRepository
{
    public ShipmentRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<Shipment?> GetByOrderId(Guid orderId)
    {
        return await _appDbContext.Shipments.FirstOrDefaultAsync(x => x.OrderId == orderId);
    }
}
