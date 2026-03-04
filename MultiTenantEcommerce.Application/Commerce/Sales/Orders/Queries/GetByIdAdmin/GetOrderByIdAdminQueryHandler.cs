using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.Payment.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Integration.Proxies;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetByIdAdmin;

public class GetOrderByIdAdminQueryHandler : IQueryHandler<GetOrderByIdAdminQuery, OrderDetailAdminDTO>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerIntegrationProxy _customerIntegrationProxy;

    public GetOrderByIdAdminQueryHandler(IOrderRepository orderRepository, 
        ICustomerIntegrationProxy customerIntegrationProxy)
    {
        _orderRepository = orderRepository;
        _customerIntegrationProxy = customerIntegrationProxy;
    }

    public async Task<OrderDetailAdminDTO> Handle(GetOrderByIdAdminQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId)
                    ?? throw new Exception("Order doesnt exist");

        var customer = await _customerIntegrationProxy.GetCustomerInfoByIdAsync(order.CustomerId);

        return order.ToAdminDetailDTO(customer.Email);
    }
}