using MultiTenantEcommerce.Domain.Common;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Events.Products;
using MultiTenantEcommerce.Domain.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Entities;
public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Price Price { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CategoryId { get; private set; }
    public Stock stock {  get; private set; }

    private Product() { }

    public Product(string name, Price price, Guid categoryId, string? description, bool isActive = true)
    {
        GuardCommon.AgainstNullOrEmpty(name, nameof(name));
        GuardCommon.AgainstMaxLength(name, 100, nameof(name));
        GuardCommon.AgainstNull(price, nameof(price));
        GuardCommon.AgainstNegativeOrZero(price.Value, nameof(price));
        GuardCommon.AgainstEmptyGuid(categoryId, nameof(categoryId));

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
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
    }

    public void UpdateDescription(string description)
    {
        GuardCommon.AgainstMaxLength(description, 255, nameof(description));
        Description = description;
    }

    public void ChangePrice(Price newPrice)
    {
        Price = newPrice;
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
        Employee p = new Employee(Guid.NewGuid(),"aa" ,new Email("boas"), new Role("a"), new Password("a"));

        p.Password.UpdatePassword("");

    }
    #endregion
}
