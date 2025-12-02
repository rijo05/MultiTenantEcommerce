using MultiTenantEcommerce.Application.Common.DTOs.Address;
using MultiTenantEcommerce.Domain.Shipping.Enums;
using MultiTenantEcommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Shipping.DTOs;
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
