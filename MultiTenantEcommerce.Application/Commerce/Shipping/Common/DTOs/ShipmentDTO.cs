using MultiTenantEcommerce.Application.Commerce.Customers.Common.DTOs;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;

namespace MultiTenantEcommerce.Application.Commerce.Shipping.Common.DTOs;

public class ShipmentDTO
{
    public Guid OrderId { get; init; }
    public string TrackingNumber { get; init; }
    public AddressResponseDTO Address { get; init; }
    public ShipmentCarrier Carrier { get; init; }
    public ShipmentStatus Status { get; init; }
    public string LabelKey { get; init; }
    public Money Price { get; init; }

    public DateTime? DeliveredAt { get; init; }
    public DateTime? ShippedAt { get; init; }
    public DateTime? EstimatedDeliveryDate { get; init; }
    public TimeSpan MinTransit { get; init; }
    public TimeSpan MaxTransit { get; init; }
}