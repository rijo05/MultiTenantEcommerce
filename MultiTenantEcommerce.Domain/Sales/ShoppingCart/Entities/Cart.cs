using MultiTenantEcommerce.Domain.Catalog.Entities;
using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Sales.Orders.Entities;
using MultiTenantEcommerce.Domain.Shipping.Enums;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Sales.ShoppingCart.Entities;
public class Cart : TenantBase
{
    private List<CartItem> _items = new();

    public Guid CustomerId { get; private set; }
    public bool IsOpen { get; private set; }
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    private Cart() { }
    public Cart(Guid tenantId, Guid customerId)
        : base(tenantId)
    {
        CustomerId = customerId;
        IsOpen = true;
    }

    public void AddItem(Product product, PositiveQuantity quantity)
    {
        var existingItem = _items.Where(x => x.Product.Id == product.Id).FirstOrDefault();

        if (existingItem is not null)
            existingItem.IncreaseQuantity(quantity);
        else
        {
            var item = new CartItem(product, quantity);
            _items.Add(item);
        }
    }

    public void DecreaseItem(Guid productId, PositiveQuantity quantity)
    {
        var item = _items.Where(x => x.Product.Id == productId).FirstOrDefault()
            ?? throw new Exception("Product isnt on cart.");

        var remaining = item.Quantity.Value - quantity.Value;

        if (remaining < 0)
            throw new Exception("Invalid quantity to remove");

        if (remaining == 0)
            _items.Remove(item);
        else
            item.DecreaseQuantity(quantity);
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.Where(x => x.Product.Id == productId).FirstOrDefault()
            ?? throw new Exception("Product isnt on cart.");

        _items.Remove(item);
    }

    public void ClearCart()
    {
        _items.Clear();
    }

    public bool IsEmpty()
    {
        return _items.Count == 0;
    }

    public Order CheckOut(Address address, ShipmentCarrier carrier)
    {
        if (IsEmpty())
            throw new Exception("Cart is empty.");

        var products = _items.Select(x => (x.Product, x.Quantity)).ToList();

        var order = new Order(this.TenantId, this.CustomerId, address, products, carrier);

        return order;
    }
}
