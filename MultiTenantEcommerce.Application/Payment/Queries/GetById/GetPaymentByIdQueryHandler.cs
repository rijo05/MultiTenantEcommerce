using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.DTOs;
using MultiTenantEcommerce.Application.Payment.Mappers;
using MultiTenantEcommerce.Domain.Payment.Interfaces;

namespace MultiTenantEcommerce.Application.Payment.Queries.GetById;
public class GetPaymentByIdQueryHandler : IQueryHandler<GetPaymentByIdQuery, PaymentResponseDTO>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly PaymentMapper _paymentMapper;

    public GetPaymentByIdQueryHandler(IPaymentRepository paymentRepository,
        PaymentMapper paymentMapper)
    {
        _paymentRepository = paymentRepository;
        _paymentMapper = paymentMapper;
    }

    public async Task<PaymentResponseDTO> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId)
            ?? throw new Exception("Payment not found");

        return _paymentMapper.ToPaymentResponseDTO(payment);
    }
}
