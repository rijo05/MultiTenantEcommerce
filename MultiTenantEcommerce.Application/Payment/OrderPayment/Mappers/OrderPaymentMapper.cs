using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Mappers;
public class OrderPaymentMapper
{
    public OrderPaymentResponseDTO ToOrderPaymentResponseDTO(Domain.Payment.Entities.OrderPayment payment)
    {
        return new OrderPaymentResponseDTO
        {
            PaymentId = payment.Id,
            CustomerId = payment.CustomerId,
            OrderId = payment.OrderId,
            Total = payment.Amount.Value,
            PaymentMethod = payment.PaymentMethod.ToString(),
            Status = payment.Status.ToString(),
            TransactionId = payment.TransactionId,
            Metadata = payment.Metadata,
            CancelledAt = payment.CancelledAt,
            CompletedAt = payment.CompletedAt
        };
    }

    public List<OrderPaymentResponseDTO> ToOrderPaymentResponseDTOList(IEnumerable<Domain.Payment.Entities.OrderPayment> payments)
    {
        return payments.Select(x => ToOrderPaymentResponseDTO(x)).ToList();
    }
}
