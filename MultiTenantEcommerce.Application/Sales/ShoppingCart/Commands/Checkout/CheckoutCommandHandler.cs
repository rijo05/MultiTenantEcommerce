using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Application.Payment.DTOs;
using MultiTenantEcommerce.Application.Payment.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Payment.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Checkout;
public class CheckoutCommandHandler : ICommandHandler<CheckoutCommand, PaymentResultDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStockService _stockService;
    private readonly IPaymentProviderFactory _paymentProviderFactory;
    private readonly ITenantContext _tenantContext;


    public CheckoutCommandHandler(ICartRepository cartRepository,
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IStockService stockService,
        IPaymentProviderFactory paymentProviderFactory,
        ITenantContext tenantContext)
    {
        _cartRepository = cartRepository;
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _stockService = stockService;
        _paymentProviderFactory = paymentProviderFactory;
        _tenantContext = tenantContext;
    }
    public async Task<PaymentResultDTO> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.customerId)
            ?? throw new Exception("Cart not found");

        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var order = cart.CheckOut(new Address(
                    request.Address.Street,
                    request.Address.City,
                    request.Address.PostalCode,
                    request.Address.Country,
                    request.Address.HouseNumber
));
            await _orderRepository.AddAsync(order);

            bool reserved = await _stockService.TryReserveStockWithRetries([.. cart.Items]);
            if (!reserved)
                throw new Exception("Could not reserve all items in cart");

            var provider = _paymentProviderFactory.GetProvider(request.PaymentMethod);

            var payment = new Domain.Payment.Entities.Payment(request.customerId,
                _tenantContext.TenantId,
                PaymentReason.Order,
                order.Id,
                order.Price,
                request.PaymentMethod);

            await _paymentRepository.AddAsync(payment);

            //CALCULAR SHIPPING E ADICIONAR AO TOTAL TODO() ###########

            var paymentURL = await provider.CreatePaymentAsync(payment.Id, order, _tenantContext.StripeAccountId);

            await _unitOfWork.CommitAsync();
            await transaction.CommitAsync();

            return paymentURL;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
