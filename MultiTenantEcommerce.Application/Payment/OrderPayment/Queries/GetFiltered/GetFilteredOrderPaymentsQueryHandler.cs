using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Mappers;
using MultiTenantEcommerce.Domain.Payment.Interfaces;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Queries.GetFiltered;
public class GetFilteredOrderPaymentsQueryHandler : IQueryHandler<GetFilteredOrderPaymentsQuery, List<OrderPaymentResponseDTO>>
{
    private readonly IOrderPaymentRepository _paymentRepository;
    private readonly OrderPaymentMapper _paymentMapper;

    public GetFilteredOrderPaymentsQueryHandler(IOrderPaymentRepository paymentRepository,
        OrderPaymentMapper paymentMapper)
    {
        _paymentRepository = paymentRepository;
        _paymentMapper = paymentMapper;
    }

    public async Task<List<OrderPaymentResponseDTO>> Handle(GetFilteredOrderPaymentsQuery request, CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetFilteredAsync(
            request.CustomerId,
            request.OrderId,
            request.Status,
            request.Method,
            request.MinCreatedAt,
            request.MaxCreatedAt,
            request.Page,
            request.PageSize,
            request.Sort
        );

        return _paymentMapper.ToOrderPaymentResponseDTOList(payments);

    }
}
