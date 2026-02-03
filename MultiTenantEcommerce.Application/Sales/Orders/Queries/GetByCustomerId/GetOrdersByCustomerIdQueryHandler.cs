using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Application.Sales.Orders.Mappers;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.Orders.Queries.GetByCustomerId;
public class GetOrdersByCustomerIdQueryHandler : IQueryHandler<GetOrdersByCustomerIdQuery, List<OrderResponseDTO>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly OrderMapper _orderMapper;

    public GetOrdersByCustomerIdQueryHandler(IOrderRepository orderRepository,
        OrderMapper orderMapper)
    {
        _orderRepository = orderRepository;
        _orderMapper = orderMapper;
    }

    public async Task<List<OrderResponseDTO>> Handle(GetOrdersByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByCustomerId(
            request.customerId,
            request.Page,
            request.PageSize,
            request.Sort);

        return _orderMapper.ToOrderResponseDTOList(orders);
    }
}
