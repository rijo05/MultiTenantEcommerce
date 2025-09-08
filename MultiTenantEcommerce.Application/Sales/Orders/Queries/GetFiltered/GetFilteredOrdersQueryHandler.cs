using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Application.Sales.Orders.Mappers;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.Orders.Queries.GetFiltered;
public class GetFilteredOrdersQueryHandler : IQueryHandler<GetFilteredOrdersQuery, List<OrderResponseDTO>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly OrderMapper _orderMapper;

    public GetFilteredOrdersQueryHandler(IOrderRepository orderRepository, OrderMapper orderMapper)
    {
        _orderRepository = orderRepository;
        _orderMapper = orderMapper;
    }


    public async Task<List<OrderResponseDTO>> Handle(GetFilteredOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetFilteredAsync(
            request.CustomerId,
            request.Status,
            request.MinDate,
            request.MaxDate,
            request.MinPrice,
            request.MaxPrice,
            request.Page,
            request.PageSize,
            request.SortOptions);

        return _orderMapper.ToOrderResponseDTOList(orders);
    }
}
