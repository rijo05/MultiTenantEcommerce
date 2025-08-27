using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Domain.Common.Guard;
using System.ComponentModel.DataAnnotations;

namespace MultiTenantEcommerce.Domain.Inventory.Entities;
public class Stock : TenantBase
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public int MinimumQuantity { get; private set; }
    public int Reserved {  get; private set; }
    public int StockAvailableAtMoment => Quantity - Reserved;

    [Timestamp]
    public byte[] RowVersion { get; set; }

    private readonly List<IDomainEvent> _domainEvents = new();

    private Stock() { }
    public Stock(Product product, Guid tenantId, int quantity = 0, int minimumQuantity = 10) : base(tenantId)
    {
        GuardCommon.AgainstNegative(quantity, nameof(quantity));
        GuardCommon.AgainstNegative(minimumQuantity, nameof(minimumQuantity));

        ProductId = product.Id;
        Quantity = quantity;
        MinimumQuantity = minimumQuantity;
        Reserved = 0;
    }


    #region STOCK CHANGES
    public void ChangeStock(int quantity)
    {
        if (quantity < 0)
        {
            var absQuantidade = Math.Abs(quantity);
            if (absQuantidade > quantity)
                throw new InvalidOperationException("Not enough stock");
        }

        Quantity += quantity;
        TriggerDomainEvents(Quantity);
        SetUpdatedAt();
    }

    public void SetStock(int quantity)
    {
        GuardCommon.AgainstNegative(quantity, nameof(quantity));
        Quantity = quantity;
        TriggerDomainEvents(Quantity);
        SetUpdatedAt();
    }

    public void SetMinimumStockLevel(int quantity)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));
        MinimumQuantity = quantity;
        SetUpdatedAt();
    }

    #endregion

    #region STOCK CONFLICTS
    public void ReleaseStock(int quantity)
    {
        if (quantity <= 0 || quantity > Reserved)
            throw new Exception("Quantity not valid");

        Reserved -= quantity;
        SetUpdatedAt();
    }
    public void ReserveStock(int quantity)
    {
        if (quantity <= 0 || quantity > Quantity - Reserved)
            throw new Exception("Quantity not valid");

        Reserved += quantity;
        SetUpdatedAt();
    }

    public void CommitStock(int quantity)
    {
        if (quantity <= 0 || quantity > Reserved)
            throw new Exception("Quantity not valid");

        Quantity -= quantity;
        Reserved -= quantity;
        SetUpdatedAt();
    }
    #endregion


    #region PRIVATES
    private void TriggerDomainEvents(int stock)
    {
        //if (stock == 0)
        //    _domainEvents.Add(new ProductOutOfStockEvent(Id));
        //else if (stock <= MinimumQuantity)
        //    _domainEvents.Add(new ProductStockLowEvent(Id));
    }
    #endregion
}
