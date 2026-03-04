using MultiTenantEcommerce.Application.Commerce.Customers.Common.Mappers;
using MultiTenantEcommerce.Application.Commerce.Shipping.Common.DTOs;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Entities;

namespace MultiTenantEcommerce.Application.Commerce.Shipping.Common.Mappers;

public class ShipmentMapper
{
    private readonly CustomerAddressMapper _addressMapper;

    public ShipmentMapper(CustomerAddressMapper addressMapper)
    {
        _addressMapper = addressMapper;
    }

    public ShipmentDTO ToShipmentDTO(Shipment shipment)
    {
        return new ShipmentDTO
        {
            OrderId = shipment.OrderId,
            Address = _addressMapper.ToAddressResponseFromDTO(shipment.Address),
            Carrier = shipment.Carrier,
            LabelKey = shipment.LabelKey,
            TrackingNumber = shipment.TrackingNumber,
            Status = shipment.Status,
            Price = shipment.Price,
            ShippedAt = shipment.ShippedAt,
            EstimatedDeliveryDate = shipment.EstimatedDeliveryDate,
            DeliveredAt = shipment.DeliveredAt,
            MinTransit = shipment.MinTransit,
            MaxTransit = shipment.MaxTransit
        };
    }
}