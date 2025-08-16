using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Entities;
public class StockMovement
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public DateTime Date { get; private set; }
    public StockMovementReason Reason {  get; private set; }
    public string? OtherReason { get; private set; }

    public StockMovement(Guid productId, int quantity, DateTime date, StockMovementReason reason, string? otherReason = null)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));
        GuardCommon.AgainstEmptyGuid(productId, nameof(productId));

        if (reason == StockMovementReason.Other && string.IsNullOrWhiteSpace(otherReason))
            throw new ArgumentException("OtherReason must be provided when 'Other' is selected.");

        if (reason is not StockMovementReason.Other && !string.IsNullOrWhiteSpace(otherReason))
            throw new Exception("OtherReason should not be provided unless 'Other' is selected.");

        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        Date = DateTime.UtcNow;
        Reason = reason;
        OtherReason = otherReason;
    }
}
