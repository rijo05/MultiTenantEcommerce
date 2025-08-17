using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Entities;
public class Product : TenantBase
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? SKU { get; private set; }
    public Money Price { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CategoryId { get; private set; }

    private Product() { }
    public Product(Guid tenantId, string name, Money price, Guid categoryId, string? sku, string? description, bool isActive = true) : base(tenantId)
    {
        GuardCommon.AgainstNullOrEmpty(name, nameof(name));
        GuardCommon.AgainstMaxLength(name, 100, nameof(name));
        GuardCommon.AgainstEmptyGuid(categoryId, nameof(categoryId));
        GuardCommon.AgainstEmptyGuid(tenantId, nameof(tenantId));

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        SKU = sku;
        Price = price;
        IsActive = isActive;
        CategoryId = categoryId;
    }

    #region UPDATE DATA
    public void UpdateName(string newName)
    {
        GuardCommon.AgainstNullOrEmpty(newName, nameof(newName));
        GuardCommon.AgainstMaxLength(newName, 100, nameof(newName));
        Name = newName;
        SetUpdatedAt();
    }

    public void UpdateDescription(string description)
    {
        GuardCommon.AgainstMaxLength(description, 255, nameof(description));
        Description = description;
        SetUpdatedAt();
    }

    public void ChangePrice(decimal newPrice)
    {
        Price.UpdatePrice(newPrice);
        SetUpdatedAt();
    }

    public void UpdateCategory(Guid newCategoryId)
    {
        GuardCommon.AgainstEmptyGuid(newCategoryId, nameof(newCategoryId));
        CategoryId = newCategoryId;
        SetUpdatedAt();
    }

    public void ChangeActive(bool isActive)
    {
        IsActive = isActive;
        SetUpdatedAt();
    }

    public void ClearDescription()
    {
        Description = null;
        SetUpdatedAt();
    }
    #endregion
}
