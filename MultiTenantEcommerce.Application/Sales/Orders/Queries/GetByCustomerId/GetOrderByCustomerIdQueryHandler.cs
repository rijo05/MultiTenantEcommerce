using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Application.Sales.Orders.Mappers;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.Orders.Queries.GetByCustomerId;
public class GetOrderByCustomerIdQueryHandler : IQueryHandler<GetOrderByCustomerIdQuery, List<OrderResponseDTO>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly OrderMapper _orderMapper;

    public GetOrderByCustomerIdQueryHandler(IOrderRepository orderRepository,
        OrderMapper orderMapper)
    {
        _orderRepository = orderRepository;
        _orderMapper = orderMapper;
    }

    public async Task<List<OrderResponseDTO>> Handle(GetOrderByCustomerIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByCustomerIdWithItems(request.customerId);

        return _orderMapper.ToOrderResponseDTOList(orders);
    }
}
