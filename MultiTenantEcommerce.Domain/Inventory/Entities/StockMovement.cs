using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Inventory.Entities;
public class StockMovement : TenantBase
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public StockMovementReason Reason { get; private set; }
    public string? OtherReason { get; private set; }

    private StockMovement() { }
    public StockMovement(Guid tenantId, Guid productId, int quantity, StockMovementReason reason, string? otherReason = null) : base(tenantId)
    {
        GuardCommon.AgainstEmptyGuid(productId, nameof(productId));

        if (reason == StockMovementReason.Other && string.IsNullOrWhiteSpace(otherReason))
            throw new ArgumentException("OtherReason must be provided when 'Other' is selected.");

        if (reason is not StockMovementReason.Other && !string.IsNullOrWhiteSpace(otherReason))
            throw new Exception("OtherReason should not be provided unless 'Other' is selected.");

        ProductId = productId;
        Quantity = quantity;
        Reason = reason;
        OtherReason = otherReason;
    }
}
