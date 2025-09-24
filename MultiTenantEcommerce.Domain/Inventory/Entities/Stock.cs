using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Inventory.Events;
using MultiTenantEcommerce.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace MultiTenantEcommerce.Domain.Inventory.Entities;
public class Stock : TenantBase
{
    public Guid ProductId { get; private set; }
    public NonNegativeQuantity Quantity { get; private set; }
    public NonNegativeQuantity MinimumQuantity { get; private set; }
    public NonNegativeQuantity Reserved { get; private set; }
    public NonNegativeQuantity StockAvailableAtMoment => Quantity - Reserved;

    [Timestamp]
    public byte[] RowVersion { get; set; }

    private Stock() { }
    public Stock(Product product, Guid tenantId, int? quantity = null, int? minimumQuantity = null)
        : base(tenantId)
    {
        ProductId = product.Id;
        Quantity = new NonNegativeQuantity(quantity ?? 0);
        MinimumQuantity = new NonNegativeQuantity(minimumQuantity ?? 10);
        Reserved = new NonNegativeQuantity(0);

        RowVersion = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
    }


    #region STOCK CHANGES
    public void IncrementStock(int quantity)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));

        Quantity = new NonNegativeQuantity(Quantity.Value + quantity);
        SetUpdatedAt();

        TriggerStockMovementEvent(quantity, StockMovementReason.Purchase);
        TriggerStockEvents();
    }

    public void DecrementStock(int quantity, StockMovementReason movementReason)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));

        if (quantity > Quantity.Value)
            throw new InvalidOperationException("Not enough stock available to remove.");

        Quantity -= new NonNegativeQuantity(quantity);
        SetUpdatedAt();

        TriggerStockMovementEvent(-quantity, movementReason);
        TriggerStockEvents();
    }

    public void SetStock(int quantity)
    {
        Quantity = new NonNegativeQuantity(quantity);
        SetUpdatedAt();

        TriggerStockMovementEvent(quantity, StockMovementReason.Adjustment);
    }

    public void SetMinimumStockLevel(int quantity)
    {
        MinimumQuantity = new NonNegativeQuantity(quantity);
        SetUpdatedAt();
    }

    #endregion

    #region STOCK CONFLICTS
    public void ReleaseReservedStock(int quantity)
    {
        var quantityToReserve = new NonNegativeQuantity(quantity);

        if (quantityToReserve.Value > Reserved.Value)
            throw new Exception("Quantity not valid");

        Reserved -= quantityToReserve;
        SetUpdatedAt();
    }
    public void ReserveStock(int quantity)
    {
        var quantityToReserve = new NonNegativeQuantity(quantity);

        if (quantityToReserve.Value > StockAvailableAtMoment.Value)
            throw new Exception("Quantity not valid");

        Reserved = new NonNegativeQuantity(Reserved.Value + quantityToReserve.Value);
        SetUpdatedAt();
    }
    public void CommitStock(int quantity)
    {
        DecrementStock(quantity, StockMovementReason.Sale);
        ReleaseReservedStock(quantity);
    }

    public bool CheckAvailability(int quantity)
    {
        return StockAvailableAtMoment.Value - quantity >= 0;
    }
    #endregion

    #region Private
    private void TriggerStockMovementEvent(int quantityChange, StockMovementReason reason)
    {
        AddDomainEvent(new StockMovementEvent(this.TenantId, this.ProductId, quantityChange, reason));
    }

    private void TriggerStockEvents()
    {
        if (Quantity.Value < MinimumQuantity.Value)
            AddDomainEvent(new LowStockEvent(this.TenantId, this.ProductId, Quantity.Value, MinimumQuantity.Value));

        if (Quantity.Value == 0)
            AddDomainEvent(new OutOfStockEvent(this.TenantId, this.ProductId));
    }

    #endregion
}
