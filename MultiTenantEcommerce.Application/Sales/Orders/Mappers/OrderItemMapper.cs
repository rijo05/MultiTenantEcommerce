using MultiTenantEcommerce.Application.Common.Helpers.Services;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;

namespace MultiTenantEcommerce.Application.Sales.Orders.Mappers;

public class OrderItemMapper
{
    private readonly HateoasLinkService _hateoasLinkService;

    public OrderItemMapper(HateoasLinkService hateoasLinkService)
    {
        _hateoasLinkService = hateoasLinkService;
    }

    public OrderItemResponseDTO ToOrderItemResponseDTO(OrderItem item)
    {
        return new OrderItemResponseDTO
        {
            OrderId = item.OrderId,
            Name = item.ProductName,
            ProductId = item.ProductId,
            UnitPrice = item.UnitPrice.Value,
            Quantity = item.Quantity.Value
        };
    }

    public List<OrderItemResponseDTO> ToOrderItemResponseDTOList(IEnumerable<OrderItem> items)
    {
        return items.Select(x => ToOrderItemResponseDTO(x)).ToList();
    }

    //mudar
    //private Dictionary<string, object> GenerateLinks(OrderItem item)
    //{
    //    return _hateoasLinkService.GenerateLinksCRUD(
    //                item.ProductId,
    //                "OrderItems",
    //                "GetById",
    //                "Update",
    //                "Delete"
    //    );
    //}
}
