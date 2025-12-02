using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Application.Payment.OrderPayment.DTOs;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Interfaces;
using MultiTenantEcommerce.Application.Shipping.Interfaces;
using MultiTenantEcommerce.Domain.Catalog.Interfaces;
using MultiTenantEcommerce.Domain.Payment.Entities;
using MultiTenantEcommerce.Domain.Payment.Interfaces;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Commands.Checkout;
public class CheckoutCommandHandler : ICommandHandler<CheckoutCommand, PaymentResultDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
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
        IProductRepository productRepository,
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
        _productRepository = productRepository;
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

        if (cart.IsEmpty()) 
            throw new Exception("Cart is empty");

        var productsIds = cart.Items.Select(x => x.ProductId).Distinct();
        var products = await _productRepository.GetByIdsAsync(productsIds);
        var productsDict = products.ToDictionary(p => p.Id);

        if (products.Count != productsIds.Count())
            throw new Exception("Some items in cart are no longer available.");


        var address = new Address(
                request.Address.Street,
                request.Address.City,
                request.Address.PostalCode,
                request.Address.Country,
                request.Address.HouseNumber);

        //CACHE FUTURO #############
        var shippingProvider = _shippingProviderFactory.GetProvider(request.Carrier);
        var quote = await shippingProvider.GetQuote(address);


        var orderItems = new List<(Guid, string, Money, PositiveQuantity)>();

        var subTotalValue = 0m;
        foreach (var item in cart.Items)
        {
            var product = productsDict[item.ProductId];
            subTotalValue += product.Price.Value * item.Quantity.Value;
            orderItems.Add((product.Id, product.Name, product.Price, item.Quantity));
        }

        var discountValue = 0m;
        //discountValue = discountService();
        Money finalTotalValue = new Money(subTotalValue - discountValue + quote.Price);


        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        Order order;
        OrderPayment payment;

        try
        {
            order = new Order(_tenantContext.TenantId,
                request.CustomerId,
                address,
                request.Carrier,
                orderItems);

            var stockRequest = cart.Items
                    .Select(i => (i.ProductId, i.Quantity.Value))
                    .ToList();

            bool reserved = await _stockService.TryReserveStock(stockRequest);

            if (!reserved)
                throw new Exception("Not enough stock");

            payment = new OrderPayment(_tenantContext.TenantId,
                request.CustomerId,
                order.Id,
                finalTotalValue,
                request.PaymentMethod);

            cart.ClearCart();

            await _orderRepository.AddAsync(order);
            await _paymentRepository.AddAsync(payment);

            await _unitOfWork.CommitAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }

        try
        {
            var paymentProvider = _paymentProviderFactory.GetProvider(request.PaymentMethod);
            //STRIPE ACCOUNT ID ERRADO ######### CONFIGURAR FUTURO
            var tenant = await _tenantRepository.GetByIdAsync(_tenantContext.TenantId)
                ?? throw new Exception("Tenant not found, Shouldnt happen");
            var paymentResult = await paymentProvider.CreatePaymentAsync(payment.Id, order, quote, tenant.Id.ToString());

            return paymentResult;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
