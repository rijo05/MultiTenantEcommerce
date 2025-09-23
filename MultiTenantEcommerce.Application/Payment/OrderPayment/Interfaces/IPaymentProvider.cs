using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Interfaces;
public interface IPaymentProvider
{
    Task<PaymentResultDTO> CreatePaymentAsync(Guid PaymentId, Order order, string tenantPaymentProviderAccountId);
}
