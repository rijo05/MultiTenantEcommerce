using MultiTenantEcommerce.Domain.Common;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Events.Products;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Entities;
public class Product
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? SKU { get; private set; }
    public Price Price { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CategoryId { get; private set; }
    public Stock stock {  get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Product() { }

    public Product(Guid tenantId, string name, Price price, Guid categoryId, string? sku, string? description, bool isActive = true)
    {
        GuardCommon.AgainstNullOrEmpty(name, nameof(name));
        GuardCommon.AgainstMaxLength(name, 100, nameof(name));
        GuardCommon.AgainstEmptyGuid(categoryId, nameof(categoryId));
        GuardCommon.AgainstEmptyGuid(tenantId, nameof(tenantId));

        Id = Guid.NewGuid();
        TenantId = tenantId;
        Name = name;
        Description = description;
        SKU = sku;
        Price = price;
        IsActive = isActive;
        CategoryId = categoryId;
        CreatedAt = DateTime.UtcNow;
    }

    #region UPDATE DATA
    public void UpdateName(string newName)
    {
        GuardCommon.AgainstNullOrEmpty(newName, nameof(newName));
        GuardCommon.AgainstMaxLength(newName, 100, nameof(newName));
        Name = newName;
    }

    public void UpdateDescription(string description)
    {
        GuardCommon.AgainstMaxLength(description, 255, nameof(description));
        Description = description;
    }

    public void ChangePrice(decimal newPrice)
    {
        Price.UpdatePrice(newPrice);
    }

    public void UpdateCategory(Guid newCategoryId)
    {
        GuardCommon.AgainstEmptyGuid(newCategoryId, nameof(newCategoryId));

        CategoryId = newCategoryId;
    }

    public void ChangeActive(bool isActive)
    {
        IsActive = isActive;
    }

    public void ClearDescription()
    {
        Description = null;
    }
    #endregion
}
