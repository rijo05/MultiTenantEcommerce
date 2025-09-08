using MultiTenantEcommerce.Application.Common.DTOs.Address;

namespace MultiTenantEcommerce.Application.Sales.Orders.DTOs;

public class OrderResponseDTO
{
    public Guid Id { get; init; }
    public Guid? CustomerId { get; init; }
    public string OrderStatus { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public AddressResponseDTO Address { get; init; }
    public List<OrderItemResponseDTO> Items { get; init; }
    public decimal TotalPrice { get; init; }
}
