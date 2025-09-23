using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Mappers;
using MultiTenantEcommerce.Domain.Payment.Interfaces;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Queries.GetById;
public class GetOrderPaymentByIdQueryHandler : IQueryHandler<GetOrderPaymentByIdQuery, OrderPaymentResponseDTO>
{
    private readonly IOrderPaymentRepository _paymentRepository;
    private readonly OrderPaymentMapper _paymentMapper;

    public GetOrderPaymentByIdQueryHandler(IOrderPaymentRepository paymentRepository,
        OrderPaymentMapper paymentMapper)
    {
        _paymentRepository = paymentRepository;
        _paymentMapper = paymentMapper;
    }

    public async Task<OrderPaymentResponseDTO> Handle(GetOrderPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId)
            ?? throw new Exception("Payment not found");

        if (request.CustomerId != null && payment.CustomerId != request.CustomerId)
            throw new Exception("You can only access your payments");

        return _paymentMapper.ToOrderPaymentResponseDTO(payment);
    }
}
