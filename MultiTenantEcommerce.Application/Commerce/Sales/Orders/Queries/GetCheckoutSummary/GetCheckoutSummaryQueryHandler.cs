using MediatR.Wrappers;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Integration.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.Orders.Queries.GetCheckoutSummary;
public class GetCheckoutSummaryQueryHandler : IQueryHandler<GetCheckoutSummaryQuery, CheckoutSummaryDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICatalogIntegrationProxy _catalogIntegrationProxy;
    private readonly ICustomerIntegrationProxy _customerIntegrationProxy;
    private readonly ILogisticsIntegrationProxy _stockIntegrationProxy;

    public GetCheckoutSummaryQueryHandler(ICartRepository cartRepository, 
        ICatalogIntegrationProxy catalogIntegrationProxy, 
        ICustomerIntegrationProxy customerIntegrationProxy, 
        ILogisticsIntegrationProxy stockIntegrationProxy)
    {
        _cartRepository = cartRepository;
        _catalogIntegrationProxy = catalogIntegrationProxy;
        _customerIntegrationProxy = customerIntegrationProxy;
        _stockIntegrationProxy = stockIntegrationProxy;
    }

    public async Task<CheckoutSummaryDTO> Handle(GetCheckoutSummaryQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerIntegrationProxy.GetCustomerInfoByIdAsync(request.CustomerId) 
            ?? throw new Exception("Customer doesnt exist");

        var cart = await _cartRepository.GetByCustomerIdAsync(request.CustomerId);

        if (cart == null || cart.IsEmpty())
            throw new Exception("Invalid cart");

        var productsIds = cart.Items.Select(x => x.ProductId).Distinct();
        var products = await _catalogIntegrationProxy.GetProductsByIds(productsIds);

        var stocks = await _stockIntegrationProxy.GetStocksByProductIdsAsync(products.Select(x => x.Id));

        decimal calculatedSubTotal = 0m;
        var unavailableItems = new List<UnavailableItemDTO>();

        foreach (var cartItem in cart.Items)
        {
            var catalogItem = products.FirstOrDefault(c => c.Id == cartItem.ProductId);

            var currentStock = stocks.FirstOrDefault(s => s.ProductId == cartItem.ProductId);

            if (catalogItem == null || currentStock == null)
            {
                unavailableItems.Add(new UnavailableItemDTO(cartItem.ProductId, "Produto Indisponível", cartItem.Quantity.Value, 0));
                continue;
            }

            if (currentStock.AvailableQuantity < cartItem.Quantity.Value)
            {
                unavailableItems.Add(new UnavailableItemDTO(
                    cartItem.ProductId, catalogItem.Name, cartItem.Quantity.Value, currentStock.AvailableQuantity));
                continue;
            }

            calculatedSubTotal += (cartItem.Quantity.Value * catalogItem.Price);
        }

        decimal shippingQuote = await _stockIntegrationProxy.CalculateQuoteAsync(request.Carrier.ToString(), customer.addresses);

        bool isStockAvailable = !unavailableItems.Any();
        decimal totalPrice = calculatedSubTotal + shippingQuote;

        return new CheckoutSummaryDTO(
            calculatedSubTotal, 
            shippingQuote, 
            totalPrice,
            isStockAvailable,
            unavailableItems);
    }
}
