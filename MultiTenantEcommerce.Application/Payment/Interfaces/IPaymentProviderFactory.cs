using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Payment.Interfaces;
public interface IPaymentProviderFactory
{
    IPaymentProvider GetProvider(PaymentMethod method);
}
