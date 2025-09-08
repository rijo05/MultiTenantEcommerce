using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Sales.Orders.Entities;
public class Order : TenantBase
{
    public Guid CustomerId { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public Address Address { get; private set; }
    public Money Price { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    private readonly List<OrderItem> _items = new();

    private Order() { }
    public Order(Guid tenantId, Guid customerId, Address address)
        : base(tenantId)
    {
        CustomerId = customerId;
        OrderStatus = OrderStatus.PendingPayment;
        Address = address;
        Price = new Money(_items.Sum(x => x.Total));
    }


    public void ChangeStatus(string status)
    {
        if (!Enum.TryParse<OrderStatus>(status, true, out var newStatus))
            throw new Exception("Invalid order status");

        OrderStatusTransitionValidator.ValidateTransition(OrderStatus, newStatus);
        OrderStatus = newStatus;

        SetUpdatedAt();

        TriggerDomainEvents(newStatus);
    }

    public void AddItem(Product product, PositiveQuantity quantity)
    {
        var orderItem = new OrderItem(Id, TenantId, product, quantity);
        _items.Add(orderItem);
    }

    #region PRIVATES
    private void TriggerDomainEvents(OrderStatus newStatus)
    {
        switch (newStatus)
        {
            case OrderStatus.Processing:
                //_domainEvents.Add(new OrderShippedEvent(this.Id));
                break;
            case OrderStatus.Shipped:
                //_domainEvents.Add(new OrderShippedEvent(this.Id));
                break;
            case OrderStatus.Delivered:
                //_domainEvents.Add(new OrderDeliveredEvent(this.Id));
                break;
            case OrderStatus.Invoiced:
                //_domainEvents.Add(new OrderShippedEvent(this.Id));
                break;
            case OrderStatus.Cancelled:
                //_domainEvents.Add(new OrderCancelledEvent(this.Id));
                break;
        }
    }

    #endregion
}
