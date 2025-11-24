using MultiTenantEcommerce.Domain.Shipping.Enums;

namespace MultiTenantEcommerce.Application.Shipping.Interfaces;
public interface IShippingProviderFactory
{
    IShippingProvider GetProvider(ShipmentCarrier carrier);
}
