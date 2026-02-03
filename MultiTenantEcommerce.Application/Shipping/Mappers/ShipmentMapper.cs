using MultiTenantEcommerce.Application.Common.Helpers.Mappers;
using MultiTenantEcommerce.Application.Shipping.DTOs;
using MultiTenantEcommerce.Domain.Shipping.Entities;

namespace MultiTenantEcommerce.Application.Shipping.Mappers;
public class ShipmentMapper
{
    private readonly AddressMapper _addressMapper;

    public ShipmentMapper(AddressMapper addressMapper)
    {
        _addressMapper = addressMapper;
    }

    public ShipmentDTO ToShipmentDTO(Shipment shipment)
    {
        return new ShipmentDTO()
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
