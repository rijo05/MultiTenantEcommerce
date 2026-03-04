using MultiTenantEcommerce.Application.Commerce.Shipping.Common.DTOs;
using MultiTenantEcommerce.Application.Commerce.Shipping.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using QuestPDF.Fluent;

namespace MultiTenantEcommerce.Infrastructure.Shipping;

public class FakeCTTProvider : IShippingProvider
{
    private readonly TimeSpan MAX_TRANSIT = TimeSpan.FromDays(5);
    private readonly TimeSpan MIN_TRANSIT = TimeSpan.FromDays(3);
    private readonly decimal PRICE = 3.99M;

    public async Task<ShipmentResultDTO> CreateShipment(Order order, string customerName)
    {
        var trackingNumber = Guid.NewGuid().ToString().Substring(0, 8);

        var document = new GenerateLabel("CTT", trackingNumber, "barcode", customerName, order.Address);
        var byteArray = document.GeneratePdf();

        await Task.Delay(50);

        return new ShipmentResultDTO(trackingNumber,
            "ctt/" + trackingNumber,
            byteArray,
            PRICE,
            MIN_TRANSIT,
            MAX_TRANSIT);
    }

    public async Task<ShippingQuoteDTO> GetQuote(Address address)
    {
        await Task.Delay(50);

        return new ShippingQuoteDTO(ShipmentCarrier.CTT, PRICE, MIN_TRANSIT, MAX_TRANSIT);
    }
}