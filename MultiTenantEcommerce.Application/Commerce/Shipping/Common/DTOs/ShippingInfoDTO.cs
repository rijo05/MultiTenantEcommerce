using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;

namespace MultiTenantEcommerce.Application.Commerce.Shipping.Common.DTOs;

public record ShippingQuoteDTO(
    ShipmentCarrier Carrier,
    decimal Price,
    TimeSpan MinTransit,
    TimeSpan MaxTransit);