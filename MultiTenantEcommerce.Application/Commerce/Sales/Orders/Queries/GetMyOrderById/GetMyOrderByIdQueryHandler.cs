using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.Mappers;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetMyOrderById;
public class GetMyOrderByIdQueryHandler : IQueryHandler<GetMyOrderByIdQuery, OrderDetailDTO>
{
    private readonly IOrderRepository _orderRepository;

    public GetMyOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDetailDTO> Handle(GetMyOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order == null || order.CustomerId != request.CustomerId)
            throw new Exception("Order doesnt exist");

        return order.ToDetailDTO();
    }
}
