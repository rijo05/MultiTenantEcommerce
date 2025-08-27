using MultiTenantEcommerce.Application.DTOs.OrderItems;
using MultiTenantEcommerce.Application.Services;
using MultiTenantEcommerce.Domain.Sales.Entities;

namespace MultiTenantEcommerce.Application.Mappers;

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
            ProductId = item.ProductId,
            Name = item.Name,
            UnitPrice = item.UnitPrice.Value,
            Quantity = item.Quantity
        };
    }

    public List<OrderItemResponseDTO> ToOrderItemResponseDTOList(List<OrderItem> items)
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
