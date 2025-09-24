using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Payment.Entities;
using MultiTenantEcommerce.Domain.Sales.Orders.Events;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Sales.Orders.Entities;
public class Order : TenantBase
{
    public Guid CustomerId { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public Address Address { get; private set; }
    public Money Price { get; private set; }
    public OrderPayment OrderPayment { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    private readonly List<OrderItem> _items = new();

    private Order() { }
    public Order(Guid tenantId, Guid customerId, Address address, IEnumerable<(Product, PositiveQuantity)> products)
        : base(tenantId)
    {
        CustomerId = customerId;
        OrderStatus = OrderStatus.PendingPayment;
        Address = address;

        foreach (var item in products)
        {
            AddItem(item.Item1, item.Item2);
        }

        Price = new Money(_items.Sum(x => x.Total));
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

    public void AddItem(Product product, PositiveQuantity quantity)
    {
        var orderItem = new OrderItem(Id, TenantId, product, quantity);
        _items.Add(orderItem);
    }

    public void AttachPayment(OrderPayment payment)
    {
        OrderPayment = payment;
    }

    #region PRIVATES
    private void TriggerDomainEvents(OrderStatus newStatus)
    {
        switch (newStatus)
        {
            case OrderStatus.Processing:
                AddDomainEvent(new OrderPaidEvent(this.TenantId, this.Id));
                break;
            case OrderStatus.Shipped:
                AddDomainEvent(new OrderShippedEvent(this.TenantId, this.Id));
                break;
            case OrderStatus.Delivered:
                AddDomainEvent(new OrderDeliveredEvent(this.TenantId, this.Id));
                break;
            case OrderStatus.Invoiced:
            //_domainEvents.Add(new OrderShippedEvent(this.Id));
            case OrderStatus.Cancelled:
                //_domainEvents.Add(new OrderShippedEvent(this.Id));
                break;
            case OrderStatus.Failed:
                AddDomainEvent(new OrderPaymentFailedEvent(this.TenantId, this.Id));
                break;
        }
    }

    #endregion
}
