using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;

namespace MultiTenantEcommerce.Application.Commerce.Shipping.Interfaces;

public interface IShippingProviderFactory
{
    IShippingProvider GetProvider(ShipmentCarrier carrier);
}