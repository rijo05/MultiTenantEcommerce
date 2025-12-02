using MultiTenantEcommerce.Application.Catalog.Products.Mappers;
using MultiTenantEcommerce.Application.Sales.ShoppingCart.DTOs;
using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Inventory.Entities;
using MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Sales.ShoppingCart.Mappers;
public class CartMapper
{
    private readonly ProductMapper _productMapper;

    public CartMapper(ProductMapper productMapper)
    {
        _productMapper = productMapper;
    }

    public CartResponseDTO ToCartResponseDTO(
            Cart cart,
            IEnumerable<Product> products,
            IEnumerable<Stock> stocks,
            Dictionary<string, string> signedUrls)
    {
        var productsDict = products.ToDictionary(p => p.Id);
        var stocksDict = stocks.ToDictionary(s => s.ProductId);

        var itemDtos = new List<CartItemResponseDTO>();

        foreach (var item in cart.Items)
        {
            var product = productsDict.TryGetValue(item.ProductId, out var p) ? p : null;
            var stock = stocksDict.TryGetValue(item.ProductId, out var s) ? s : null;

            if (product == null || stock == null) continue;

            var productDto = _productMapper.ToProductResponseDTO(product, stock, signedUrls);

            itemDtos.Add(new CartItemResponseDTO
            {
                Product = productDto,
                Quantity = item.Quantity.Value
            });
        }

        return new CartResponseDTO
        {
            CustomerId = cart.CustomerId,
            CreatedAt = cart.CreatedAt,
            UpdatedAt = cart.UpdatedAt,
            CartItems = itemDtos
        };
    }
}
