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

    private List<ProductImages> _images = new List<ProductImages>();
    public IReadOnlyCollection<ProductImages> Images => _images.AsReadOnly();

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
    Guid? CategoryId,
    Category? category)
    {
        if (CategoryId.HasValue)
            UpdateCategory(CategoryId.Value, category);


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

    public void UpdateCategory(Guid newCategoryId, Category category)
    {
        GuardCommon.AgainstEmptyGuid(newCategoryId, nameof(newCategoryId));
        CategoryId = newCategoryId;
        Category = category;
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

    public ProductImages AddImage(string fileName, long size, string contentType, bool isMain = false)
    {
        if (isMain)
        {
            foreach (var image in _images)
                image.UnmarkAsMain();
        }
        else if (_images.Count == 0)
            isMain = true;

        var newImage = new ProductImages(this.TenantId, this.Id, fileName, size, contentType, isMain);
        _images.Add(newImage);

        return newImage;
    }

    public void DeleteImage(string key)
    {
        var image = _images.FirstOrDefault(x => x.Key == key)!;

        _images.Remove(image);

        if (image.IsMain && _images.Count > 0)
            _images[0].MarkAsMain();
    }

    public void MarkAsMain(string key)
    {
        var image = _images.FirstOrDefault(x => x.Key == key)!;

        image.MarkAsMain();
    }
}
