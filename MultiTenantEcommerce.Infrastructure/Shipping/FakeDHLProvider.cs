using MultiTenantEcommerce.Application.Shipping.DTOs;
using MultiTenantEcommerce.Application.Shipping.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Shipping.Enums;
using MultiTenantEcommerce.Domain.ValueObjects;
using QuestPDF.Fluent;

namespace MultiTenantEcommerce.Infrastructure.Shipping;
public class FakeDHLProvider : IShippingProvider
{
    private readonly TimeSpan MIN_TRANSIT = TimeSpan.FromDays(2);
    private readonly TimeSpan MAX_TRANSIT = TimeSpan.FromDays(4);
    private readonly decimal PRICE = 4.99M;

    public async Task<ShipmentResultDTO> CreateShipment(Order order, string customerName)
    {
        var trackingNumber = Guid.NewGuid().ToString().Substring(0, 8);

        var document = new GenerateLabel("DHL", trackingNumber, "barcode", customerName, order.Address);
        var byteArray = document.GeneratePdf();

        await Task.Delay(50);

        return new ShipmentResultDTO(trackingNumber,
            "dhl/" + trackingNumber,
            byteArray,
            PRICE,
            MIN_TRANSIT,
            MAX_TRANSIT);
    }

    public async Task<ShippingQuoteDTO> GetQuote(Address address)
    {
        await Task.Delay(50);

        return new ShippingQuoteDTO(ShipmentCarrier.DHL, PRICE, MIN_TRANSIT, MAX_TRANSIT);
    }
}
