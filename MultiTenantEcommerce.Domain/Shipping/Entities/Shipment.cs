using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Shipping.Enums;
using MultiTenantEcommerce.Domain.Shipping.Events;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Shipping.Entities;
public class Shipment : TenantBase
{
    public Guid OrderId { get; private set; }
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

    private Shipment() { }
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
                AddDomainEvent(new ShipmentShippedEvent(this.TenantId, this.Id));
                break;
            case ShipmentStatus.Delivered:
                AddDomainEvent(new ShipmentDeliveredEvent(this.TenantId, this.Id));
                break;
        }
    }
}
