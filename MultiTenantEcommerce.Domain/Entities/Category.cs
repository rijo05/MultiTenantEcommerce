using MultiTenantEcommerce.Domain.Common.Guard;

namespace MultiTenantEcommerce.Domain.Entities;
public class Category
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    private Category() { }


    public Category(Guid tenantId, string name, string? description)
    {
        GuardCommon.AgainstNullOrEmpty(name, nameof(name));
        GuardCommon.AgainstMaxLength(description, 255, nameof(description));
        GuardCommon.AgainstEmptyGuid(tenantId, nameof(tenantId));

        TenantId = tenantId;
        Name = name;
        Description = description;
        IsActive = true;
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

    public void ClearDescription()
    {
        Description = null;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
    #endregion
}
