using MultiTenantEcommerce.Domain.Commerce.Sales.Orders.Entities;
using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Commerce.Sales.ShoppingCart.Entities;

public class Cart : TenantBase
{
    public Guid? CustomerId { get; private set; }
    public Guid? AnonymousId { get; private set; }
    public bool IsOpen { get; private set; }
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
    private readonly List<CartItem> _items = new();

    private Cart()
    {
    }

    public Cart(Guid tenantId, Guid? customerId, Guid? anonymousId) : base(tenantId)
    {
        if (customerId.HasValue && anonymousId.HasValue)
            throw new Exception("O carrinho não pode pertencer a um cliente e a um anónimo em simultâneo.");
        if (!customerId.HasValue && !anonymousId.HasValue)
            throw new Exception("O carrinho tem de pertencer a alguém (Cliente ou Anónimo).");

        CustomerId = customerId;
        AnonymousId = anonymousId;
        IsOpen = true;
    }

    public void AssignToCustomer(Guid customerId)
    {
        if (CustomerId.HasValue && CustomerId.Value != customerId)
            throw new Exception("Este carrinho já pertence a um cliente diferente.");

        CustomerId = customerId;
        AnonymousId = null;
        SetUpdatedAt();
    }

    public void AddItem(Guid productId, PositiveQuantity quantity)
    {
        if (!IsOpen) 
            throw new Exception("Carrinho fechado.");

        var existingItem = _items.FirstOrDefault(x => x.ProductId == productId);

        if (existingItem is not null)
            existingItem.IncreaseQuantity(quantity);
        else
            _items.Add(new CartItem(TenantId, Id, productId, quantity));

        SetUpdatedAt();
    }

    public void DecreaseItem(Guid productId, PositiveQuantity quantity)
    {
        if (!IsOpen) 
            throw new Exception("Carrinho fechado.");

        var item = _items.FirstOrDefault(x => x.ProductId == productId)
                   ?? throw new Exception("Product isnt on cart.");

        var remaining = item.Quantity.Value - quantity.Value;

        if (remaining < 0)
            throw new Exception("Invalid quantity to remove");

        if (remaining == 0)
            _items.Remove(item);
        else
            item.DecreaseQuantity(quantity);

        SetUpdatedAt();
    }

    public void RemoveItem(Guid productId)
    {
        if (!IsOpen) 
            throw new Exception("Carrinho fechado.");

        var item = _items.FirstOrDefault(x => x.ProductId == productId) 
            ?? throw new Exception("Produto não encontrado.");

        _items.Remove(item);
        SetUpdatedAt();
    }

    public void ClearCart()
    {
        if (!IsOpen) 
            throw new Exception("Carrinho fechado.");

        _items.Clear();
        SetUpdatedAt();
    }

    public void Close()
    {
        IsOpen = false;
        SetUpdatedAt();
    }

    public bool IsEmpty()
    {
        return _items.Count == 0;
    }
}