using MultiTenantEcommerce.Application.DTOs.Address;
using MultiTenantEcommerce.Application.DTOs.OrderItems;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.DTOs.Order;

public class CreateOrderDTO
{
    public Guid CustomerId { get; set; }
    public CreateAddressDTO Address { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<CreateOrderItemDTO> Items { get; set; }
}
