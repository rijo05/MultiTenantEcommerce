using MultiTenantEcommerce.Application.Payment.DTOs;

namespace MultiTenantEcommerce.Application.Sales.Orders.DTOs;
public class OrderResponseWithPayment
{
    public OrderResponseDTO Order { get; set; }
    public PaymentResponseDTO Payment { get; set; }
}
