using Microsoft.Extensions.DependencyInjection;
using MultiTenantEcommerce.Application.Shipping.Interfaces;
using MultiTenantEcommerce.Domain.Shipping.Enums;

namespace MultiTenantEcommerce.Infrastructure.Shipping;
public class ShippingProviderFactory : IShippingProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ShippingProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IShippingProvider GetProvider(ShipmentCarrier carrier)
    {
        return carrier switch
        {
            ShipmentCarrier.DHL => _serviceProvider.GetRequiredService<FakeDHLProvider>(),
            ShipmentCarrier.UPS => _serviceProvider.GetRequiredService<FakeUPSProvider>(),
            ShipmentCarrier.CTT => _serviceProvider.GetRequiredService<FakeCTTProvider>(),
            _ => throw new NotImplementedException("Not implemented")
        };
    }
}
