using MultiTenantEcommerce.Application.Commerce.Shipping.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Shipping.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;
using MultiTenantEcommerce.Infrastructure.Shared.Shipping;
using MultiTenantEcommerce.Infrastructure.Shipping;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using QuestPDF.Fluent;

namespace MultiTenantEcommerce.Infrastructure.Commerce.Shipping.Shipping;

public class FakeUPSProvider : IShippingProvider
{
    private readonly TimeSpan MAX_TRANSIT = TimeSpan.FromDays(8);
    private readonly TimeSpan MIN_TRANSIT = TimeSpan.FromDays(5);
    private readonly decimal PRICE = 2.99M;

    public async Task<ShipmentResultDTO> CreateShipment(Order order, string customerName)
    {
        var trackingNumber = Guid.NewGuid().ToString().Substring(0, 8);

        var document = new GenerateLabel("UPS", trackingNumber, "barcode", customerName, order.Address);
        var byteArray = document.GeneratePdf();

        await Task.Delay(50);

        return new ShipmentResultDTO(trackingNumber,
            "ups/" + trackingNumber,
            byteArray,
            PRICE,
            MIN_TRANSIT,
            MAX_TRANSIT);
    }

    public async Task<ShippingQuoteDTO> GetQuote(Address address)
    {
        await Task.Delay(50);

        return new ShippingQuoteDTO(ShipmentCarrier.UPS, PRICE, MIN_TRANSIT, MAX_TRANSIT);
    }
}