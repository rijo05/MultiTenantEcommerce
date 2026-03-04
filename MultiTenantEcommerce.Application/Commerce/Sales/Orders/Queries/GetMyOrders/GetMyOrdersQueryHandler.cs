using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetMyOrders;
public class GetMyOrdersQueryHandler : IQueryHandler<GetMyOrdersQuery, PaginatedList<OrderSummaryDTO>>
{
    private readonly IOrderRepository _orderRepository;

    public GetMyOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<PaginatedList<OrderSummaryDTO>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetByCustomerId(
            request.CustomerId, 
            request.Page, 
            request.PageSize, 
            request.SortOptions);

        return orders.ToPaginatedSummaryDTO();
    }
}
