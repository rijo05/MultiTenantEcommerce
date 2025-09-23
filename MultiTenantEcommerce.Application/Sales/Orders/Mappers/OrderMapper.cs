using MultiTenantEcommerce.Application.Common.Helpers.Mappers;
using MultiTenantEcommerce.Application.Common.Helpers.Services;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Mappers;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;

namespace MultiTenantEcommerce.Application.Sales.Orders.Mappers;

public class OrderMapper
{
    private readonly HateoasLinkService _hateoasLinkService;
    private readonly OrderItemMapper _orderItemMapper;
    private readonly AddressMapper _addressMapper;
    private readonly OrderPaymentMapper _paymentMapper;

    public OrderMapper(HateoasLinkService hateoasLinkService,
        OrderItemMapper orderItemMapper,
        AddressMapper addressMapper,
        OrderPaymentMapper paymentMapper)
    {
        _hateoasLinkService = hateoasLinkService;
        _orderItemMapper = orderItemMapper;
        _addressMapper = addressMapper;
        _paymentMapper = paymentMapper;
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
            TotalPrice = order.Items.Sum(x => x.UnitPrice.Value * x.Quantity.Value),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
        };
    }

    public List<OrderResponseDTO> ToOrderResponseDTOList(IEnumerable<Order> orders)
    {
        return orders.Select(x => ToOrderResponseDTO(x)).ToList();
    }

    public OrderResponseWithPayment ToOrderResponseWithPaymentDTO(Order order)
    {
        return new OrderResponseWithPayment()
        {
            Order = ToOrderResponseDTO(order),
            Payment = _paymentMapper.ToOrderPaymentResponseDTO(order.OrderPayment)
        };
    }

    public List<OrderResponseWithPayment> ToOrderResponseWithPaymentDTOList(IEnumerable<Order> orders)
    {
        return orders.Select(x => ToOrderResponseWithPaymentDTO(x)).ToList();
    }


    //private Dictionary<string, object> GenerateLinks(Order order)
    //{
    //    return _hateoasLinkService.GenerateLinksCRUD(
    //                order.Id,
    //                "Orders",
    //                "GetById",
    //                "Update",
    //                "Delete"
    //    );
    //}
}
