using MultiTenantEcommerce.Application.DTOs.Order;
using MultiTenantEcommerce.Application.Services;
using MultiTenantEcommerce.Domain.Entities;
using MultiTenantEcommerce.Application.DTOs.Address;

namespace MultiTenantEcommerce.Application.Mappers;

public class OrderMapper
{
    private readonly HateoasLinkService _hateoasLinkService;
    private readonly OrderItemMapper _orderItemMapper;
    private readonly AddressMapper _addressMapper;

    public OrderMapper(HateoasLinkService hateoasLinkService, OrderItemMapper orderItemMapper, AddressMapper addressMapper)
    {
        _hateoasLinkService = hateoasLinkService;
        _orderItemMapper = orderItemMapper;
        _addressMapper = addressMapper;
    }

    public OrderResponseDTO ToOrderResponseDTO(Order order)
    {
        return new OrderResponseDTO
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderStatus = order.OrderStatus,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            PayedAt = order.PayedAt,
            Address = _addressMapper.ToAddressResponseFromDTO(order.Address),
            Items = _orderItemMapper.ToOrderItemResponseDTOList(order.Items.ToList())
        };
    }

    public List<OrderResponseDTO> ToOrderResponseDTOList(List<Order> orders)
    {
        return orders.Select(x => ToOrderResponseDTO(x)).ToList();
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
