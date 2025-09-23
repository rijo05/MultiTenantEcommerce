using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;

namespace MultiTenantEcommerce.Application.Sales.Orders.DTOs;
public class OrderResponseWithPayment
{
    public OrderResponseDTO Order { get; set; }
    public OrderPaymentResponseDTO Payment { get; set; }
}
