using MultiTenantEcommerce.Application.DTOs.Order;
using MultiTenantEcommerce.Application.Services;
using MultiTenantEcommerce.Application.DTOs.OrderItems;
using MultiTenantEcommerce.Domain.Sales.Entities;

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

    public OrderResponseDTO ToOrderResponseDTO(Order order, List<OrderItemResponseDTO> items)
    {
        return new OrderResponseDTO
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderStatus = order.OrderStatus.ToString(),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            PayedAt = order.PayedAt,
            Address = _addressMapper.ToAddressResponseFromDTO(order.Address),
            Items = items,
            TotalPrice = items.Sum(x => x.Total)
        };
    }

    public List<MultipleOrderResponseDTO> ToMultipleOrderResponseDTOList(List<Order> orders)
    {
        var list = new List<MultipleOrderResponseDTO>();

        foreach (var order in orders)
        {
            list.Add(new MultipleOrderResponseDTO
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderStatus = order.OrderStatus.ToString(),
                PayedAt = order.PayedAt,
                TotalPrice = order.Price.Value,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            });
        }

        return list;
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
