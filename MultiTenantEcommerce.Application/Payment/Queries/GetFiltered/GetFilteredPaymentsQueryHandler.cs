using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.DTOs;
using MultiTenantEcommerce.Application.Payment.Mappers;
using MultiTenantEcommerce.Domain.Payment.Interfaces;

namespace MultiTenantEcommerce.Application.Payment.Queries.GetFiltered;
public class GetFilteredPaymentsQueryHandler : IQueryHandler<GetFilteredPaymentsQuery, List<PaymentResponseDTO>>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly PaymentMapper _paymentMapper;

    public GetFilteredPaymentsQueryHandler(IPaymentRepository paymentRepository,
        PaymentMapper paymentMapper)
    {
        _paymentRepository = paymentRepository;
        _paymentMapper = paymentMapper;
    }

    public async Task<List<PaymentResponseDTO>> Handle(GetFilteredPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetFilteredAsync(
            request.PayerId,
            request.PayeeId,
            request.Status,
            request.MinDate,
            request.MaxDate,
            request.Reason,
            request.ReasonId,
            request.Page,
            request.PageSize,
            request.Sort
        );

        return _paymentMapper.ToPaymentResponseDTOList(payments);

    }
}
