using MultiTenantEcommerce.Domain.Commerce.Inventory.Enums;
using MultiTenantEcommerce.Domain.Commerce.Inventory.Events;
using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Utilities.Guards;

namespace MultiTenantEcommerce.Domain.Commerce.Inventory.Entities;

public class Stock : TenantBase
{
    private Stock()
    {
    }

    public Stock(Guid tenantId, Guid productId, int? quantity = null, int? minimumQuantity = null)
        : base(tenantId)
    {
        ProductId = productId;
        Quantity = new NonNegativeQuantity(quantity ?? 0);
        MinimumQuantity = new NonNegativeQuantity(minimumQuantity ?? 0);
        Reserved = new NonNegativeQuantity(0);
    }

    public Guid ProductId { get; }
    public NonNegativeQuantity Quantity { get; private set; }
    public NonNegativeQuantity MinimumQuantity { get; private set; }
    public NonNegativeQuantity Reserved { get; private set; }
    public NonNegativeQuantity StockAvailableAtMoment => Quantity - Reserved;


    #region STOCK CHANGES

    public void IncrementStock(int quantity)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));

        var availableBefore = StockAvailableAtMoment.Value;
        var minBefore = MinimumQuantity.Value;

        Quantity = new NonNegativeQuantity(Quantity.Value + quantity);
        SetUpdatedAt();

        TriggerStockMovementEvent(quantity, StockMovementReason.Purchase);
        CheckAndTriggerThresholdEvent(availableBefore, minBefore);
    }

    public void DecrementStock(int quantity, StockMovementReason movementReason)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));
        if (quantity > Quantity.Value)
            throw new InvalidOperationException("Not enough stock available to remove.");

        var availableBefore = StockAvailableAtMoment.Value;
        var minBefore = MinimumQuantity.Value;

        Quantity -= new NonNegativeQuantity(quantity);
        SetUpdatedAt();

        TriggerStockMovementEvent(-quantity, movementReason);
        CheckAndTriggerThresholdEvent(availableBefore, minBefore);
    }

    public void SetStock(int quantity)
    {
        GuardCommon.AgainstNegative(quantity, nameof(quantity));

        var availableBefore = StockAvailableAtMoment.Value;
        var minBefore = MinimumQuantity.Value;

        var diff = quantity - Quantity.Value;

        Quantity = new NonNegativeQuantity(quantity);
        SetUpdatedAt();

        TriggerStockMovementEvent(diff, StockMovementReason.Adjustment);
        CheckAndTriggerThresholdEvent(availableBefore, minBefore);
    }

    public void SetMinimumStockLevel(int quantity)
    {
        GuardCommon.AgainstNegative(quantity, nameof(quantity));

        var availableBefore = StockAvailableAtMoment.Value;
        var minBefore = MinimumQuantity.Value;

        MinimumQuantity = new NonNegativeQuantity(quantity);
        SetUpdatedAt();

        CheckAndTriggerThresholdEvent(availableBefore, minBefore);
    }

    #endregion

    #region STOCK CONFLICTS

    public void ReleaseReservedStock(int quantity)
    {
        var quantityToRelease = new NonNegativeQuantity(quantity);
        if (quantityToRelease.Value > Reserved.Value)
            throw new InvalidOperationException("Quantity not valid");

        var availableBefore = StockAvailableAtMoment.Value;
        var minBefore = MinimumQuantity.Value;

        Reserved -= quantityToRelease;
        SetUpdatedAt();

        CheckAndTriggerThresholdEvent(availableBefore, minBefore);
    }

    public void ReserveStock(int quantity)
    {
        var quantityToReserve = new NonNegativeQuantity(quantity);
        if (quantityToReserve.Value > StockAvailableAtMoment.Value)
            throw new InvalidOperationException("Quantity not valid");

        var availableBefore = StockAvailableAtMoment.Value;
        var minBefore = MinimumQuantity.Value;

        Reserved = new NonNegativeQuantity(Reserved.Value + quantityToReserve.Value);
        SetUpdatedAt();

        CheckAndTriggerThresholdEvent(availableBefore, minBefore);
    }

    public void CommitStock(int quantity)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));
        var quantityToCommit = new NonNegativeQuantity(quantity);

        if (quantityToCommit.Value > Reserved.Value)
            throw new InvalidOperationException("Cannot commit more than reserved.");

        Quantity -= quantityToCommit;
        Reserved -= quantityToCommit;
        SetUpdatedAt();

        TriggerStockMovementEvent(-quantity, StockMovementReason.Sale);
    }

    public bool CheckAvailability(int quantity)
    {
        return StockAvailableAtMoment.Value - quantity >= 0;
    }

    #endregion

    #region Private

    private void TriggerStockMovementEvent(int quantityChange, StockMovementReason reason)
    {
        if (quantityChange != 0)
            AddDomainEvent(new StockMovementEvent(TenantId, ProductId, quantityChange, reason));
    }

    private void CheckAndTriggerThresholdEvent(int availableBefore, int minBefore)
    {
        var availableAfter = StockAvailableAtMoment.Value;
        var minAfter = MinimumQuantity.Value;

        bool wasOutOfStock = availableBefore <= 0;
        bool isOutOfStock = availableAfter <= 0;
        bool crossedOutOfStock = wasOutOfStock != isOutOfStock;

        bool wasLowStock = availableBefore > 0 && availableBefore <= minBefore;
        bool isLowStock = availableAfter > 0 && availableAfter <= minAfter;
        bool crossedLowStock = wasLowStock != isLowStock;

        if (crossedOutOfStock || crossedLowStock)
        {
            AddDomainEvent(new StockLevelChangedEvent(
                TenantId,
                ProductId,
                availableAfter,
                minAfter));
        }
    }
    #endregion
}