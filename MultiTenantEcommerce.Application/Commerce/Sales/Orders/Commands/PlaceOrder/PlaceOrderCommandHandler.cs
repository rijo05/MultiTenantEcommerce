using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Common.Mappers;
using MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetMyOrderById;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Interfaces;
using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.ValueObjects;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using MultiTenantEcommerce.Shared.Integration.Proxies;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Commands.PlaceOrder;

public class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand, OrderDetailDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICatalogIntegrationProxy _catalogIntegrationProxy;
    private readonly ILogisticsIntegrationProxy _logisticsIntegrationProxy;
    private readonly ICustomerIntegrationProxy _customerIntegrationProxy;
    private readonly IOrderRepository _orderRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;

    public PlaceOrderCommandHandler(ICartRepository cartRepository, 
        ICatalogIntegrationProxy catalogIntegrationProxy, 
        ILogisticsIntegrationProxy logisticsIntegrationProxy, 
        ICustomerIntegrationProxy customerIntegrationProxy,
        IOrderRepository orderRepository, 
        ITenantContext tenantContext, 
        IUnitOfWork unitOfWork)
    {
        _cartRepository = cartRepository;
        _catalogIntegrationProxy = catalogIntegrationProxy;
        _logisticsIntegrationProxy = logisticsIntegrationProxy;
        _customerIntegrationProxy = customerIntegrationProxy;
        _orderRepository = orderRepository;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDetailDTO> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {

        var customer = await _customerIntegrationProxy.GetCustomerInfoByIdAsync(request.CustomerId)
                ?? throw new Exception("Customer doesnt exist");

        var orderAdress = customer.addresses.FirstOrDefault(x => x.Id == request.AddressId);
        if (orderAdress == null)
            throw new Exception("Address not valid");

        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId)
                   ?? throw new Exception("Cart not found");

        if (cart.IsEmpty())
            throw new Exception("Cart is empty");

        var productsIds = cart.Items.Select(x => x.ProductId).Distinct();
        var products = await _catalogIntegrationProxy.GetProductsByIds(productsIds);
        var productsDict = products.ToDictionary(p => p.Id);

        if (products.Count != productsIds.Count())
            throw new Exception("Some items in cart are no longer available.");

        var quotePrice = await _logisticsIntegrationProxy.CalculateQuoteAsync(request.Carrier.ToString(), orderAdress.ToString());

        var orderItems = new List<(Guid, string, Money, PositiveQuantity)>();

        var subTotalValue = 0m;
        foreach (var item in cart.Items)
        {
            var product = productsDict[item.ProductId];
            subTotalValue += product.Price * item.Quantity.Value;
            orderItems.Add((product.Id, product.Name, new Money(product.Price), item.Quantity));
        }

        Money finalTotalValue = new Money(subTotalValue + quotePrice);

        var order = new Order(_tenantContext.TenantId,
            request.CustomerId,
            new AddressSnapshot(orderAdress.Street, orderAdress.City, orderAdress.PostalCode, orderAdress.Country, orderAdress.HouseNumber),
            request.Carrier,
            new Money(quotePrice),
            orderItems);

        await _orderRepository.AddAsync(order);


        var stockRequest = cart.Items
            .Select(i => (i.ProductId, i.Quantity.Value))
            .ToList();

        cart.Close();

        try
        {
            var reserved = await _logisticsIntegrationProxy.TryReserveStockAsync(stockRequest);

            await _unitOfWork.CommitAsync();
        }
        catch (Exception)
        {
            throw;
        }

        return order.ToDetailDTO();
    }
}