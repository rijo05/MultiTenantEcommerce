using MultiTenantEcommerce.Application.Commerce.Shipping.Common.DTOs;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Shared.Integration.DTOs;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Interfaces;

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
        Guid TenantId,
        string stripeCustomerId,
        string stripePriceId,
        string successUrl,
        string cancelUrl);

    public Task<string> CreatePortalSessionAsync(string stripeCustomerId, string returnUrl);
}