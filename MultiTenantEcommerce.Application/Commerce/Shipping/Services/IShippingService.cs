using MultiTenantEcommerce.Application.Commerce.Shipping.Common.DTOs;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Entities;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;
using System.Net;

namespace MultiTenantEcommerce.Application.Commerce.Shipping.Services;

public interface IShippingService
{
    public Task<Shipment> CreateShipment(Guid tenantId, Order order, ShipmentCarrier carrier);
    public Task<List<ShippingQuoteDTO>> GetShippingQuotes(Address address);
    public Task<ShippingQuoteDTO> GetShippingQuote(ShipmentCarrier carrier, Address address);
}