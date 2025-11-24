using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Shipping.Enums;

namespace MultiTenantEcommerce.Domain.Tenants.Entities;
public class ShippingProviderConfig : TenantBase
{
    public ShipmentCarrier Carrier { get; private set; }
    public bool IsActive { get; private set; }

    internal ShippingProviderConfig(Guid tenantId, ShipmentCarrier carrier, bool isActive)
        : base(tenantId)
    {
        Carrier = carrier;
        IsActive = isActive;
    }

    internal void Activate() => IsActive = true;
    internal void Deactivate() => IsActive = false;
}
