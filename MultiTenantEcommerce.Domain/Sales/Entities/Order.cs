using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Events;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Sales.Entities;
public class Order : TenantBase
{
    public Guid CustomerId { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public DateTime? PayedAt { get; private set; }
    public Address Address { get; private set; }
    public Money Price {  get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    private readonly List<OrderItem> _items = new();

    private readonly List<IDomainEvent> _domainEvents = new();

    private Order() { }
    public Order(Guid orderId, Guid tenantId, Guid customerId, Address address, IEnumerable<OrderItem> items, PaymentMethod paymentMethod) : base(tenantId)
    {
        //ver o q fazer em relacao ao id, criar os orderitems dentro da order #########
        Id = orderId;
        CustomerId = customerId;
        OrderStatus = OrderStatus.PendingPayment;
        Address = address;
        _items.AddRange(items);
        PaymentMethod = paymentMethod;
        Price = new Money(_items.Sum(x => x.Total));
    }

    #region CHANGE ORDER STATUS

    public void ChangeStatus(OrderStatus newStatus)
    {
        OrderStatusTransitionValidator.ValidateTransition(OrderStatus, newStatus);
        OrderStatus = newStatus;
        SetUpdatedAt();

        TriggerDomainEvents(newStatus);
    }

    public void AddItem()
    {

    }
    #endregion

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
