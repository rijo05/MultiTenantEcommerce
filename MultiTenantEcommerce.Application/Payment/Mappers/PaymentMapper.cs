using MultiTenantEcommerce.Application.Payment.DTOs;

namespace MultiTenantEcommerce.Application.Payment.Mappers;
public class PaymentMapper
{
    public PaymentResponseDTO ToPaymentResponseDTO(Domain.Payment.Entities.Payment payment)
    {
        return new PaymentResponseDTO
        {
            PaymentId = payment.Id,
            PayerId = payment.PayerId,
            PayeeId = payment.PayeeId,
            Amount = payment.Amount.Value,
            PaymentMethod = payment.PaymentMethod.ToString(),
            Reason = payment.Reason.ToString(),
            ReasonId = payment.ReasonId,
            Status = payment.Status.ToString(),
            TransactionId = payment.TransactionId,
            Metadata = payment.Metadata,
            CancelledAt = payment.CancelledAt,
            CompletedAt = payment.CompletedAt,
        };
    }

    public List<PaymentResponseDTO> ToPaymentResponseDTOList(IEnumerable<Domain.Payment.Entities.Payment> payments)
    {
        return payments.Select(x => ToPaymentResponseDTO(x)).ToList();
    }
}
