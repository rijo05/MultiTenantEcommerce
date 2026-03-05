using Microsoft.Extensions.DependencyInjection;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Enums;

namespace MultiTenantEcommerce.Infrastructure.Shared.Payments;

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