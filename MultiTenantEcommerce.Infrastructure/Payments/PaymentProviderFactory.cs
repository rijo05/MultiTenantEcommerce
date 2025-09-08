using Microsoft.Extensions.DependencyInjection;
using MultiTenantEcommerce.Application.Payment.Interfaces;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Infrastructure.Payments;
public class PaymentProviderFactory : IPaymentProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPaymentProvider GetProvider(PaymentMethod method)
    {
        return method switch
        {
            PaymentMethod.Stripe => _serviceProvider.GetRequiredService<StripePaymentProvider>(),
            _ => throw new NotSupportedException()
        };
    }
}
