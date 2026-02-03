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
        string tenantPaymentProviderAccountId,
        decimal applicationFeeAmount);

    public Task<string> CreateCustomerAsync(string name, string email, Guid tenantId);

    public Task<string> CreateCheckoutSessionAsync(
        string stripeCustomerId,
        string stripePriceId,
        string successUrl,
        string cancelUrl);

    public Task<string> CreatePortalSessionAsync(string stripeCustomerId, string returnUrl);
}
