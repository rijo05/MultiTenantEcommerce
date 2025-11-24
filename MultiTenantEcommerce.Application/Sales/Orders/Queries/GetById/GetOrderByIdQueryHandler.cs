using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Sales.Orders.DTOs;
using MultiTenantEcommerce.Application.Sales.Orders.Mappers;
using MultiTenantEcommerce.Domain.Payment.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;

namespace MultiTenantEcommerce.Application.Sales.Orders.Queries.GetById;
public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderResponseWithPayment>
{
    private readonly IOrderRepository _orderRepository;
    private readonly OrderMapper _orderMapper;
    private readonly IOrderPaymentRepository _paymentRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository,
        OrderMapper orderMapper,
        IOrderPaymentRepository paymentRepository)
    {
        _orderRepository = orderRepository;
        _orderMapper = orderMapper;
        _paymentRepository = paymentRepository;
    }

    public async Task<OrderResponseWithPayment> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdIncluding(request.OrderId, x => x.Items, x => x.OrderPayment!)
            ?? throw new Exception("Order doesnt exist");

        var payment = await _paymentRepository.GetByOrderId(request.OrderId)
            ?? throw new Exception("Payment couldnt be found");

        if (request.customerId != null && order.CustomerId != request.customerId)
            throw new Exception("This order doesnt belong to you");

        return _orderMapper.ToOrderResponseWithPaymentDTO(order);
    }
}
