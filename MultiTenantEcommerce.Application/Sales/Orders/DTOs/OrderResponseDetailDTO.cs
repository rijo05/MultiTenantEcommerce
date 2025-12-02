using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Shipping.DTOs;

namespace MultiTenantEcommerce.Application.Sales.Orders.DTOs;
public class OrderResponseDetailDTO : OrderResponseDTO
{
    public OrderPaymentResponseDTO? Payment { get; set; }
    public ShipmentDTO? Shipping { get; set; }
}
