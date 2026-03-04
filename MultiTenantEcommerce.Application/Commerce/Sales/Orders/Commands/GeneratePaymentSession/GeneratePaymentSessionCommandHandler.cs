using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using MultiTenantEcommerce.Shared.Integration.DTOs;
using MultiTenantEcommerce.Shared.Integration.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.GeneratePaymentSession;
public class GeneratePaymentSessionCommandHandler : ICommandHandler<GeneratePaymentSessionCommand, PaymentResultDTO>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentIntegrationProxy _paymentProxy;
    private readonly IUnitOfWork _unitOfWork;

    public GeneratePaymentSessionCommandHandler(IOrderRepository orderRepository, 
        IPaymentIntegrationProxy paymentProxy, 
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _paymentProxy = paymentProxy;
        _unitOfWork = unitOfWork;
    }

    public async Task<PaymentResultDTO> Handle(GeneratePaymentSessionCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId);

        if (order == null || order.CustomerId != request.CustomerId)
            throw new Exception("Order doesnt exist");

        var payment = order.GetOrCreatePaymentAttempt();

        if (!string.IsNullOrEmpty(payment.StripeSessionId) && !string.IsNullOrEmpty(payment.StripeSessionURL))
            return new PaymentResultDTO(payment.StripeSessionURL, payment.StripeSessionId);

        var stripeResponse = await _paymentProxy.CreatePaymentSessionAsync(
            payment.Id, 
            order.Id, 
            payment.Amount.Value, 
            order.TenantId);

        order.SetOrUpdatePaymentSession(stripeResponse.PaymentId, stripeResponse.PaymentURL);

        await _unitOfWork.CommitAsync();

        return stripeResponse;
    }
}
