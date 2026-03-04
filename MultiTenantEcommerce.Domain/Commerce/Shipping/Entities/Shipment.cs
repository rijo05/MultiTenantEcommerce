using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;
using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Commerce.Shipping.Entities;

public class Shipment : TenantBase
{
    private Shipment()
    {
    }

    public Shipment(Guid TenantId,
        Guid orderId,
        string trackingNumber,
        Address address,
        //GeoLocation geoLocation,
        ShipmentCarrier carrier,
        Money price,
        TimeSpan minTransit,
        TimeSpan maxTransit) : base(TenantId)
    {
        OrderId = orderId;
        TrackingNumber = trackingNumber;
        Address = address;
        //GeoLocation = geoLocation;
        Carrier = carrier;
        Status = ShipmentStatus.Pending;
        LabelKey = $"{this.TenantId}/shipments/{OrderId}/{Guid.NewGuid()}";
        Price = price;
        MinTransit = minTransit;
        MaxTransit = maxTransit;
    }

    public Guid OrderId { get; }
    public string TrackingNumber { get; private set; }

    public Address Address { get; private set; }

    //public GeoLocation GeoLocation { get; private set; }
    public ShipmentCarrier Carrier { get; private set; }
    public ShipmentStatus Status { get; private set; }
    public string LabelKey { get; private set; }
    public Money Price { get; private set; }

    public DateTime? DeliveredAt { get; private set; }
    public DateTime? ShippedAt { get; private set; }
    public DateTime? EstimatedDeliveryDate { get; private set; }
    public TimeSpan MinTransit { get; private set; }
    public TimeSpan MaxTransit { get; private set; }

    public void MarkAsShipped(int seconds)
    {
        ChangeStatus(ShipmentStatus.Shipped);
        ShippedAt = DateTime.UtcNow;

        EstimatedDeliveryDate = ShippedAt.Value.AddSeconds(seconds);
    }

    public void MarkAsDelivered()
    {
        ChangeStatus(ShipmentStatus.Delivered);
        DeliveredAt = DateTime.UtcNow;
    }

    private void ChangeStatus(ShipmentStatus newStatus)
    {
        ShipmentStatusTransitionValidator.ValidateTransition(Status, newStatus);
        Status = newStatus;
        SetUpdatedAt();
        TriggerDomainEvents(newStatus);
    }

    private void TriggerDomainEvents(ShipmentStatus newStatus)
    {
        switch (newStatus)
        {
            case ShipmentStatus.Shipped:
                AddDomainEvent(new ShipmentShippedEvent(TenantId, Id));
                break;
            case ShipmentStatus.Delivered:
                AddDomainEvent(new ShipmentDeliveredEvent(TenantId, Id));
                break;
        }
    }
}

public static class ShipmentStatusTransitionValidator
{
    private static readonly Dictionary<ShipmentStatus, List<ShipmentStatus>> AllowedTransitions = new()
    {
        { ShipmentStatus.Pending, new List<ShipmentStatus> { ShipmentStatus.Cancelled, ShipmentStatus.Shipped } },
        { ShipmentStatus.Shipped, new List<ShipmentStatus> { ShipmentStatus.InTransit } },
        { ShipmentStatus.InTransit, new List<ShipmentStatus> { ShipmentStatus.Delivered } }
    };

    public static void ValidateTransition(ShipmentStatus CurrentStatus, ShipmentStatus newStatus)
    {
        if (AllowedTransitions.TryGetValue(CurrentStatus, out var allowed) && allowed.Contains(newStatus)) return;

        throw new Exception($"Invalid shipment status transition from {CurrentStatus} to {newStatus}.");
    }
}