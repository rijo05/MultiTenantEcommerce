using MultiTenantEcommerce.Domain.Commerce.Shipping.Entities;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Commerce.Shipping.Interfaces;

public interface IShipmentRepository : IRepository<Shipment>
{
    public Task<Shipment?> GetByOrderId(Guid orderId);
}