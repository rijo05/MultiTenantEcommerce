using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Enums;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Events;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Validator;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.ValueObjects;
using MultiTenantEcommerce.Domain.Commerce.Shipping.Enums;
using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;

public class Order : TenantBase
{
    public Guid CustomerId { get; private set; }
    public AddressSnapshot Address { get; private set; }

    public Money ShippingCost { get; private set; }
    public Money SubTotal { get; private set; }
    public Money TotalPrice { get; private set; }

    public ShipmentCarrier ShipmentCarrier { get; private set; }
    public OrderStatus OrderStatus { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    private readonly List<OrderItem> _items = new();

    public OrderPayment? Payment { get; private set; }

    public DateTime ExpiresAt {  get; private set; }

    private Order() { }

    public Order(Guid tenantId, Guid customerId,
        AddressSnapshot address,
        ShipmentCarrier carrier,
        Money shippingCost,
        IEnumerable<(Guid ProductId, string Name, Money UnitPrice, PositiveQuantity Quantity)> itemsData) : base(tenantId)
    {
        CustomerId = customerId;
        OrderStatus = OrderStatus.PendingPayment;
        Address = address;
        ShipmentCarrier = carrier;
        ShippingCost = shippingCost;
        ExpiresAt = CreatedAt.AddMinutes(10);

        foreach (var item in itemsData)
            _items.Add(new OrderItem(Id, TenantId, item.ProductId, item.Name, item.UnitPrice, item.Quantity));

        SubTotal = new Money(_items.Sum(x => x.Total.Value));
        TotalPrice = new Money(SubTotal.Value + ShippingCost.Value);
    }


    public void ChangeStatus(OrderStatus newStatus)
    {
        OrderStatusTransitionValidator.ValidateTransition(OrderStatus, newStatus);
        OrderStatus = newStatus;
        SetUpdatedAt();
        TriggerDomainEvents(newStatus);
    }

    public void MarkAsPaid(string transactionId)
    {
        if (Payment == null)
            throw new Exception("Payment doesnt exist");

        if (Payment.Status != PaymentStatus.AwaitingPayment || OrderStatus != OrderStatus.PendingPayment)
            throw new Exception("Estado inválido");

        Payment.MarkAsCompleted(transactionId);
        ChangeStatus(OrderStatus.Processing);
    }

    public void MarkAsCancelled(string? reason)
    {
        if (Payment == null || Payment.Status != PaymentStatus.AwaitingPayment || OrderStatus != OrderStatus.PendingPayment)
            return;

        Payment.MarkAsFailed(reason);
        ChangeStatus(OrderStatus.Cancelled);
    }


    public OrderPayment GetOrCreatePaymentAttempt()
    {
        if (OrderStatus == OrderStatus.Processing || OrderStatus == OrderStatus.Completed)
            throw new Exception("Esta encomenda já foi paga.");

        if (OrderStatus == OrderStatus.Cancelled)
            throw new Exception("Não é possível pagar uma encomenda cancelada.");

        if (DateTime.UtcNow > ExpiresAt)
        {
            ChangeStatus(OrderStatus.Cancelled);
            throw new Exception("O tempo limite para pagar expirou.");
        }

        if (Payment != null && Payment.Status == PaymentStatus.AwaitingPayment)
            return Payment;

        Payment = new OrderPayment(TenantId, CustomerId, Id, TotalPrice);
        return Payment;
    }

    public void SetOrUpdatePaymentSession(string sessionId, string sessionUrl)
    {
        if (Payment == null)
            throw new Exception("Payment doesnt exist");

        Payment.UpdateStripeSessionId(sessionId);
        Payment.UpdateStripeSessionURL(sessionUrl);
        SetUpdatedAt();
    }

    #region PRIVATES

    private void TriggerDomainEvents(OrderStatus newStatus)
    {
        switch (newStatus)
        {
            case OrderStatus.Processing:
                AddDomainEvent(new OrderPaidEvent(TenantId, Id));
                break;
            case OrderStatus.Failed:
                AddDomainEvent(new OrderPaymentFailedEvent(TenantId, Id));
                break;
        }
    }

    #endregion
}