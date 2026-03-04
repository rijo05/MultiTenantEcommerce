using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;
using MultiTenantEcommerce.Shared.Domain.Abstractions;

namespace MultiTenantEcommerce.Domain.Commerce.Shipping.Entities;

public class ShippingProviderConfig : TenantBase
{
    private ShippingProviderConfig()
    {
    }

    internal ShippingProviderConfig(Guid tenantId, ShipmentCarrier carrier, bool isActive)
        : base(tenantId)
    {
        Carrier = carrier;
        IsActive = isActive;
    }

    public ShipmentCarrier Carrier { get; private set; }
    public bool IsActive { get; private set; }

    internal void Activate()
    {
        IsActive = true;
    }

    internal void Deactivate()
    {
        IsActive = false;
    }
}