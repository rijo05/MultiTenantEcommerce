using MultiTenantEcommerce.Domain.Common;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Entities;
public class Order : TenantBase, IHasDomainEvents
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public DateTime? PayedAt { get; private set; }
    public Address Address { get; private set; }
    public Money Price {  get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    private readonly List<OrderItem> _items = new();

    #region domain events
    private readonly List<IDomainEvent> _domainEvents = new();
    public List<IDomainEvent> DomainEvents => _domainEvents;
    public void ClearDomainEvents() => _domainEvents.Clear();
    #endregion

    private Order() { }
    public Order(Guid orderId, Guid tenantId, Guid customerId, Address address, IEnumerable<OrderItem> items, PaymentMethod paymentMethod) : base(tenantId)
    {
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
        OrderStatusTransitionValidator.ValidateTransition(this.OrderStatus, newStatus);
        OrderStatus = newStatus;
        SetUpdatedAt();

        TriggerDomainEvents(newStatus);
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
