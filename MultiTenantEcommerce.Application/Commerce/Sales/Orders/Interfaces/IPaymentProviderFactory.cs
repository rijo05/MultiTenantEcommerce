using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Enums;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Interfaces;

public interface IPaymentProviderFactory
{
    IPaymentProvider GetProvider(PaymentMethod method);
}