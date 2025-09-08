using MultiTenantEcommerce.Application.Payment.DTOs;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;

namespace MultiTenantEcommerce.Application.Payment.Interfaces;
public interface IPaymentProvider
{
    Task<PaymentResultDTO> CreatePaymentAsync(Guid PaymentId, Order order, string tenantPaymentProviderAccountId);
}
