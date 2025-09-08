using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Catalog.Entities;
public class Product : TenantBase
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Money Price { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }

    private Product() { }
    public Product(Guid tenantId,
        string name,
        Money price,
        Category category,
        string? description,
        bool? isActive) : base(tenantId)
    {
        GuardCommon.AgainstNullOrEmpty(name, nameof(name));
        GuardCommon.AgainstMaxLength(name, 100, nameof(name));
        GuardCommon.AgainstEmptyGuid(tenantId, nameof(tenantId));

        Name = name;
        Description = description;
        Price = price;
        IsActive = isActive ?? true;
        CategoryId = category.Id;
        Category = category;
    }

    #region UPDATE DATA

    public void UpdateProduct(string? Name,
    string? Description,
    decimal? Price,
    bool? IsActive,
    Guid? CategoryId)
    {
        if (CategoryId.HasValue)
            UpdateCategory(CategoryId.Value);


        if (!string.IsNullOrEmpty(Name))
            UpdateName(Name);

        if (Description is not null)
        {
            if (Description == "")
                ClearDescription();
            else
                UpdateDescription(Description);
        }

        if (Price.HasValue)
            ChangePrice(Price.Value);

        if (IsActive.HasValue)
            ChangeActive(IsActive.Value);
    }
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
