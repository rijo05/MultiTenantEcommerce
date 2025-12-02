using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Sales.Orders.Events;
using MultiTenantEcommerce.Domain.Shipping.Enums;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Sales.Orders.Entities;
public class Order : TenantBase
{
    public Guid CustomerId { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public Address Address { get; private set; }
    public Money Price { get; private set; }
    public ShipmentCarrier ShipmentCarrier { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    private readonly List<OrderItem> _items = new();

    private Order() { }
    public Order(Guid tenantId,
        Guid customerId,
        Address address,
        ShipmentCarrier carrier,
        IEnumerable<(Guid ProductId, string Name, Money UnitPrice, PositiveQuantity Quantity)> itemsData)
        : base(tenantId)
    {
        CustomerId = customerId;
        OrderStatus = OrderStatus.PendingPayment;
        Address = address;
        ShipmentCarrier = carrier;

        foreach (var item in itemsData)
        {
            _items.Add(new OrderItem(Id, TenantId, item.ProductId, item.Name, item.UnitPrice, item.Quantity));
        }

        Price = new Money(_items.Sum(x => x.Total.Value));
    }


    public void ChangeStatus(OrderStatus newStatus)
    {
        OrderStatusTransitionValidator.ValidateTransition(OrderStatus, newStatus);
        OrderStatus = newStatus;
        SetUpdatedAt();
        TriggerDomainEvents(newStatus);
    }

    public void MarkAsPaid()
    {
        ChangeStatus(OrderStatus.Processing);
    }

    public void MarkAsFailed()
    {
        ChangeStatus(OrderStatus.Failed);
    }

    #region PRIVATES
    private void TriggerDomainEvents(OrderStatus newStatus)
    {
        switch (newStatus)
        {
            case OrderStatus.Processing:
                AddDomainEvent(new OrderPaidEvent(this.TenantId, this.Id));
                break;
            case OrderStatus.Failed:
                AddDomainEvent(new OrderPaymentFailedEvent(this.TenantId, this.Id));
                break;
        }
    }
    #endregion
}
