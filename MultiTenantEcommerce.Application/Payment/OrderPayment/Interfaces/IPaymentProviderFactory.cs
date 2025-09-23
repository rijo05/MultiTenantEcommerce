using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Interfaces;
public interface IPaymentProviderFactory
{
    IPaymentProvider GetProvider(PaymentMethod method);
}
