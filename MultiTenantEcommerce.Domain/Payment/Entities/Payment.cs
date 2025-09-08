using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Payment.Entities;
public class Payment : TenantBase
{
    public Guid PayerId { get; private set; }
    public Guid PayeeId { get; private set; }

    public PaymentReason Reason { get; private set; }
    public Guid ReasonId { get; private set; }  //id da order, id da compra...

    public Money Amount { get; private set; }

    public PaymentStatus Status { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }

    public string? TransactionId { get; private set; }

    public DateTime? CompletedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    public string? Metadata { get; private set; }

    private Payment() { }
    public Payment(
        Guid payerId,
        Guid payeeId,
        PaymentReason reason,
        Guid reasonId,
        Money amount,
        PaymentMethod method,
        string? metadata = null)
    {
        PayerId = payerId;
        PayeeId = payeeId;
        Reason = reason;
        ReasonId = reasonId;
        Amount = amount;
        PaymentMethod = method;
        Metadata = metadata;
        Status = PaymentStatus.AwaitingPayment;
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
