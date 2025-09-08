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
    private readonly IPaymentRepository _paymentRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository,
        OrderMapper orderMapper,
        IPaymentRepository paymentRepository)
    {
        _orderRepository = orderRepository;
        _orderMapper = orderMapper;
        _paymentRepository = paymentRepository;
    }

    public async Task<OrderResponseWithPayment> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdWithItemsAsync(request.OrderId)
            ?? throw new Exception("Order doesnt exist");

        var payment = await _paymentRepository.GetByOrderId(request.OrderId)
            ?? throw new Exception("Payment couldnt be found");

        return _orderMapper.ToOrderResponseWithPaymentDTO(order, payment);
    }
}
