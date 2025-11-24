using MultiTenantEcommerce.Application.Shipping.DTOs;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Shipping.Entities;
using MultiTenantEcommerce.Domain.Shipping.Enums;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Shipping.Interfaces;
public interface IShippingService
{
    public Task<Shipment> CreateShipment(Guid tenantId, Order order, ShipmentCarrier carrier);
    public Task<List<ShippingQuoteDTO>> GetShippingQuotes(Address address);
}
