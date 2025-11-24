using MultiTenantEcommerce.Application.Shipping.DTOs;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Shipping.Interfaces;
public interface IShippingProvider
{
    public Task<ShippingQuoteDTO> GetQuote(Address address);
    public Task<ShipmentResultDTO> CreateShipment(Order order, string customerName);
}