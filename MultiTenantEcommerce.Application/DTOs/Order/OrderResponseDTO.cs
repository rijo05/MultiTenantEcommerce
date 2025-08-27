using MultiTenantEcommerce.Application.DTOs.OrderItems;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Application.DTOs.Address;

namespace MultiTenantEcommerce.Application.DTOs.Order;

public class OrderResponseDTO
{
    public Guid Id { get; init; }
    public Guid? CustomerId { get; init; }
    public string OrderStatus { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime? PayedAt { get; init; }
    public AddressResponseDTO Address { get; init; }
    public List<OrderItemResponseDTO> Items { get; init; }
    public decimal TotalPrice { get; init; }
}
