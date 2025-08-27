using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.Catalog.Entities;
public class Category : TenantBase
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    private Category() { }
    public Category(Guid tenantId, string name, string? description, bool isActive = true) : base(tenantId)
    {
        GuardCommon.AgainstNullOrEmpty(name, nameof(name));
        GuardCommon.AgainstMaxLength(description, 255, nameof(description));

        Name = name;
        Description = description;
        IsActive = isActive;
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

    public void ClearDescription()
    {
        Description = null;
        SetUpdatedAt();
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        SetUpdatedAt();
    }
    #endregion
}
