using MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Common.DTOs;
using MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Interfaces;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using MultiTenantEcommerce.Shared.Integration.Proxies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Commerce.Sales.ShoppingCart.Queries.GetMyCart;
public class GetMyCartQueryHandler : IQueryHandler<GetMyCartQuery, CartDetailDTO>
{
    private readonly ICartRepository _cartRepository;
    private readonly ICatalogIntegrationProxy _catalogProxy;

    public GetMyCartQueryHandler(ICartRepository cartRepository, 
        ICatalogIntegrationProxy catalogProxy)
    {
        _cartRepository = cartRepository; 
        _catalogProxy = catalogProxy; 
    }

    public async Task<CartDetailDTO> Handle(GetMyCartQuery request, CancellationToken ct)
    {
        var cart = await _cartRepository.GetActiveCartAsync(request.CustomerId, request.AnonymousId);

        if (cart == null || cart.IsEmpty())
            return new CartDetailDTO(Guid.Empty, new List<CartItemDetailDTO>(), 0);

        var productIds = cart.Items.Select(x => x.ProductId).ToList();
        var products = await _catalogProxy.GetProductsByIds(productIds);

        var itemDetails = new List<CartItemDetailDTO>();
        decimal subTotal = 0;

        foreach (var item in cart.Items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null) continue;

            var linePrice = product.Price * item.Quantity.Value;
            subTotal += linePrice;

            itemDetails.Add(new CartItemDetailDTO(product.Id, product.Name, product.ImageURL, product.Price, item.Quantity.Value, linePrice));
        }

        return new CartDetailDTO(cart.Id, itemDetails, subTotal);
    }
}
