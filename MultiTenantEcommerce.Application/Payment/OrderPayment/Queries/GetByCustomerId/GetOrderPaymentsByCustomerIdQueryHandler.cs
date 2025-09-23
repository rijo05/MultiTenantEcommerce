using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Mappers;
using MultiTenantEcommerce.Domain.Payment.Interfaces;

namespace MultiTenantEcommerce.Application.Payment.OrderPayment.Queries.GetByCustomerId;
public class GetOrderPaymentsByCustomerIdQueryHandler : IQueryHandler<GetOrderPaymentsByCustomerIdQuery, List<OrderPaymentResponseDTO>>
{
    private readonly IOrderPaymentRepository _orderPaymentRepository;
    private readonly OrderPaymentMapper _orderPaymentMapper;

    public GetOrderPaymentsByCustomerIdQueryHandler(IOrderPaymentRepository orderPaymentRepository,
        OrderPaymentMapper orderPaymentMapper)
    {
        _orderPaymentRepository = orderPaymentRepository;
        _orderPaymentMapper = orderPaymentMapper;
    }

    public async Task<List<OrderPaymentResponseDTO>> Handle(GetOrderPaymentsByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var payments = await _orderPaymentRepository.GetByCustomerId(
            request.customerId,
            request.Page,
            request.PageSize,
            request.Sort);

        return _orderPaymentMapper.ToOrderPaymentResponseDTOList(payments);
    }
}
