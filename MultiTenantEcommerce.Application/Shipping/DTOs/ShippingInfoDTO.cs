using MultiTenantEcommerce.Domain.Shipping.Enums;

namespace MultiTenantEcommerce.Application.Shipping.DTOs;
public record ShippingQuoteDTO(
    ShipmentCarrier Carrier,
    decimal Price,
    TimeSpan MinTransit,
    TimeSpan MaxTransit);
