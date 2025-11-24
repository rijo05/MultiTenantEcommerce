using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Shipping.DTOs;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Interfaces;
public interface IPaymentProvider
{
    public Task<PaymentResultDTO> CreatePaymentAsync(
        Guid PaymentId,
        Order order,
        ShippingQuoteDTO shipping,
        string tenantPaymentProviderAccountId);
}
