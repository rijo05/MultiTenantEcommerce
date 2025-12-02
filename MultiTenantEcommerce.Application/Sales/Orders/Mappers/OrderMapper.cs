using MultiTenantEcommerce.Application.Common.Helpers.Mappers;
using MultiTenantEcommerce.Application.Common.Helpers.Services;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Mappers;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Application.Shipping.Mappers;
using MultiTenantEcommerce.Domain.Payment.Entities;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Shipping.Entities;

namespace MultiTenantEcommerce.Application.Sales.Orders.Mappers;

public class OrderMapper
{
    private readonly OrderItemMapper _orderItemMapper;
    private readonly AddressMapper _addressMapper;
    private readonly OrderPaymentMapper _paymentMapper;
    private readonly ShipmentMapper _shipmentMapper;

    public OrderMapper(HateoasLinkService hateoasLinkService,
        OrderItemMapper orderItemMapper,
        AddressMapper addressMapper,
        OrderPaymentMapper paymentMapper,
        ShipmentMapper shipmentMapper)
    {
        _orderItemMapper = orderItemMapper;
        _addressMapper = addressMapper;
        _paymentMapper = paymentMapper;
        _shipmentMapper = shipmentMapper;
    }

    public OrderResponseDTO ToOrderResponseDTO(Order order)
    {
        return new OrderResponseDTO
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderStatus = order.OrderStatus.ToString(),
            Address = _addressMapper.ToAddressResponseFromDTO(order.Address),
            Items = _orderItemMapper.ToOrderItemResponseDTOList(order.Items),
            TotalPrice = order.Price.Value,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
        };
    }

    public List<OrderResponseDTO> ToOrderResponseDTOList(IEnumerable<Order> orders)
    {
        return orders.Select(x => ToOrderResponseDTO(x)).ToList();
    }


    public OrderResponseDetailDTO ToOrderResponseDetailDTO(Order order, OrderPayment? payment, Shipment? shipment)
    {
        return new OrderResponseDetailDTO
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderStatus = order.OrderStatus.ToString(),
            TotalPrice = order.Price.Value,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Address = _addressMapper.ToAddressResponseFromDTO(order.Address),
            Items = _orderItemMapper.ToOrderItemResponseDTOList(order.Items),

            Payment = payment != null ? _paymentMapper.ToOrderPaymentResponseDTO(payment) : null,
            Shipping = shipment != null ? _shipmentMapper.ToShipmentDTO(shipment) : null
        };
    }
}
