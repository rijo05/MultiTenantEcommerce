using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Interfaces;
using MultiTenantEcommerce.Application.Shipping.Interfaces;
using MultiTenantEcommerce.Domain.Payment.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Checkout;
public class CheckoutCommandHandler : ICommandHandler<CheckoutCommand, PaymentResultDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly IOrderPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStockService _stockService;
    private readonly IPaymentProviderFactory _paymentProviderFactory;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IAddressValidator _addressValidator;
    private readonly IShippingProviderFactory _shippingProviderFactory;

    public CheckoutCommandHandler(ICartRepository cartRepository,
        IOrderPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IStockService stockService,
        IPaymentProviderFactory paymentProviderFactory,
        ITenantRepository tenantRepository,
        ITenantContext tenantContext,
        IAddressValidator addressValidator,
        IShippingProviderFactory shippingProvider)
    {
        _cartRepository = cartRepository;
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _stockService = stockService;
        _paymentProviderFactory = paymentProviderFactory;
        _tenantRepository = tenantRepository;
        _tenantContext = tenantContext;
        _addressValidator = addressValidator;
        _shippingProviderFactory = shippingProvider;
    }

    public async Task<PaymentResultDTO> Handle(CheckoutCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId)
            ?? throw new Exception("Cart not found");

        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        try
        {
            var address = new Address(
                    request.Address.Street,
                    request.Address.City,
                    request.Address.PostalCode,
                    request.Address.Country,
                    request.Address.HouseNumber);

            //o quote ja valida o address
            //CACHE FUTURO #############
            var shippingProvider = _shippingProviderFactory.GetProvider(request.Carrier);
            var quote = await shippingProvider.GetQuote(address);


            var order = cart.CheckOut(address, request.Carrier);
            await _orderRepository.AddAsync(order);


            bool reserved = await _stockService.TryReserveStockWithRetries([.. cart.Items]);
            if (!reserved)
                throw new Exception("Could not reserve all items in cart");


            var paymentProvider = _paymentProviderFactory.GetProvider(request.PaymentMethod);

            var payment = new Domain.Payment.Entities.OrderPayment(request.CustomerId,
                order.Id,
                _tenantContext.TenantId,
                new Money(order.Price.Value + quote.Price),
                request.PaymentMethod);

            await _paymentRepository.AddAsync(payment);


            var accountId = await _tenantRepository.GetByIdAsync(_tenantContext.TenantId);

            var paymentURL = await paymentProvider.CreatePaymentAsync(payment.Id, order, quote, accountId.Id.ToString());

            //STRIPE ACCOUNT ID ERRADO #########

            await _unitOfWork.CommitAsync();
            await transaction.CommitAsync();

            return new PaymentResultDTO { PaymentId = "", PaymentURL = "" };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
