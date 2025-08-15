using MultiTenantEcommerce.Domain.Common;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Events.Products;

namespace MultiTenantEcommerce.Domain.Entities;
public class Stock : IHasDomainEvents
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Product Product { get; private set; }
    public int Quantity { get; private set; }
    public int MinimumQuantity { get; private set; }
    public int Reserved {  get; private set; }
    public int StockAvailableAtMoment => Quantity - Reserved;


    private readonly List<IDomainEvent> _domainEvents = new();
    public List<IDomainEvent> DomainEvents => _domainEvents;
    public void ClearDomainEvents() => _domainEvents.Clear();

    public Stock(Product product, int quantity = 0, int minimumQuantity = 10)
    {
        GuardCommon.AgainstNegative(quantity, nameof(quantity));
        GuardCommon.AgainstNegative(minimumQuantity, nameof(minimumQuantity));

        Id = Guid.NewGuid();
        ProductId = product.Id;
        Quantity = quantity;
        MinimumQuantity = minimumQuantity;
        Reserved = 0;
    }

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
    }

    public void SetStock(int quantity)
    {
        GuardCommon.AgainstNegative(quantity, nameof(quantity));
        Quantity = quantity;

        TriggerDomainEvents(Quantity);
    }

    public void SetMinimumStockLevel(int quantity)
    {
        GuardCommon.AgainstNegativeOrZero(quantity, nameof(quantity));

        MinimumQuantity = quantity;
    }

    #region STOCK CONFLICTS
    public void ReleaseStock(int quantity)
    {
        if (quantity <= 0 || quantity > Reserved)
            throw new Exception("Quantity not valid");

        Reserved -= quantity;
    }
    public void ReserveStock(int quantity)
    {
        if (quantity <= 0 || quantity > Quantity - Reserved)
            throw new Exception("Quantity not valid");

        Reserved += quantity;
    }

    public void CommitStock(int quantity)
    {
        if (quantity <= 0 || quantity > Reserved)
            throw new Exception("Quantity not valid");

        Quantity -= quantity;
        Reserved -= quantity;
    }
    #endregion


    #region PRIVATES
    private void TriggerDomainEvents(int stock)
    {
        if (stock == 0)
            _domainEvents.Add(new ProductOutOfStockEvent(Id));
        else if (stock <= MinimumQuantity)
            _domainEvents.Add(new ProductStockLowEvent(Id));
    }
    #endregion
}
