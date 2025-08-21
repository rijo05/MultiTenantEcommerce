using MultiTenantEcommerce.Application.DTOs.Address;
using MultiTenantEcommerce.Application.DTOs.OrderItems;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.DTOs.Order;
public class MultipleOrderResponseDTO
{
    public Guid Id { get; init; }
    public Guid? CustomerId { get; init; }
    public string OrderStatus { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public DateTime? PayedAt { get; init; }
    public decimal TotalPrice { get; init; }
}
