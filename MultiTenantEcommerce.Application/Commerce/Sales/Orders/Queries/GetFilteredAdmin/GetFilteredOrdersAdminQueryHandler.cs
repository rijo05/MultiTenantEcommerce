using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Shared.Application;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Integration.Proxies;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetFilteredAdmin;

public class GetFilteredOrdersAdminQueryHandler : IQueryHandler<GetFilteredOrdersAdminQuery, PaginatedList<OrderSummaryAdminDTO>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerIntegrationProxy _customerIntegrationProxy;

    public GetFilteredOrdersAdminQueryHandler(IOrderRepository orderRepository, 
        ICustomerIntegrationProxy customerIntegrationProxy)
    {
        _customerIntegrationProxy = customerIntegrationProxy;
    }

    public async Task<PaginatedList<OrderSummaryAdminDTO>> Handle(GetFilteredOrdersAdminQuery request,
        CancellationToken cancellationToken)
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

        var customerIds = orders.Items.Select(o => o.CustomerId).Distinct().ToList();
        var orderIds = orders.Items.Select(o => o.Id).ToList();

        var customerNames = await _customerIntegrationProxy.GetCustomerNamesByIdsAsync(customerIds);

        return orders.ToAdminPaginatedSummaryDTO(customerNames);
    }
}