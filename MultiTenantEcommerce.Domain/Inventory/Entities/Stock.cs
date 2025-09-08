using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;
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
    }


    #region STOCK CHANGES
    public void AddStock(int quantity)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));

        Quantity = new NonNegativeQuantity(Quantity.Value + quantity);
        SetUpdatedAt();
    }

    public void RemoveStock(int quantity)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));

        if (quantity > Quantity.Value)
            throw new InvalidOperationException("Not enough stock available to remove.");

        Quantity -= new NonNegativeQuantity(quantity);
        SetUpdatedAt();
    }

    public void SetStock(int quantity)
    {
        Quantity = new NonNegativeQuantity(quantity);
        //TriggerDomainEvents(Quantity);
        SetUpdatedAt();
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
        RemoveStock(quantity);
        ReleaseReservedStock(quantity);
        SetUpdatedAt();
    }

    public bool CheckAvailability(int quantity)
    {
        return StockAvailableAtMoment.Value - quantity >= 0;
    }
    #endregion
}
