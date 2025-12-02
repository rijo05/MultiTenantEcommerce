using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Payment.Entities;
public class OrderPayment : TenantBase
{
    public Guid CustomerId { get; private set; }
    public Guid OrderId { get; private set; }

    public Money Amount { get; private set; }

    public PaymentStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }

    public string? TransactionId { get; private set; }

    public DateTime? CompletedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    public string? Metadata { get; private set; }

    private OrderPayment() { }
    public OrderPayment(Guid tenantId,
        Guid customerId,
        Guid orderId,
        Money amount,
        PaymentMethod method)
        : base(tenantId)
    {
        CustomerId = customerId;
        OrderId = orderId;
        Status = PaymentStatus.AwaitingPayment;
        Amount = amount;
        PaymentMethod = method;
    }

    public void MarkAsCompleted(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
            throw new ArgumentException("Transaction ID must be provided.");

        if (Status == PaymentStatus.Completed)
            return;

        Status = PaymentStatus.Completed;
        TransactionId = transactionId;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed(string? reason = null)
    {
        if (Status == PaymentStatus.Failed)
            return;

        Status = PaymentStatus.Failed;
        if (!string.IsNullOrEmpty(reason))
            Metadata = reason;

        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        Status = PaymentStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateMetadata(string metadata)
    {
        Metadata = metadata;
        UpdatedAt = DateTime.UtcNow;
    }
}
