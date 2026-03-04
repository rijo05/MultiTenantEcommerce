using MultiTenantEcommerce.Application.Commerce.Shipping.Common.DTOs;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Commerce.Shipping.Interfaces;

public interface IShippingProvider
{
    public Task<ShippingQuoteDTO> GetQuote(Address address);
    public Task<ShipmentResultDTO> CreateShipment(Order order, string customerName);
}