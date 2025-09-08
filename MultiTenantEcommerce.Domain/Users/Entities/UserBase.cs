using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Users.Entities;
public abstract class UserBase : TenantBase
{
    public string Name { get; private set; }
    public Password Password { get; private set; }
    public Email Email { get; protected set; }
    public bool IsActive { get; protected set; }

    protected UserBase() { }

    protected UserBase(Guid tenantId, string name, Password password, Email email)
        : base(tenantId)
    {
        Name = name;
        Password = password;
        Email = email;
        IsActive = true;
    }

    public void UpdatePassword(string? password)
    {
        Password.UpdatePassword(password);
        SetUpdatedAt();
    }

    public void UpdateEmail(string? newEmail)
    {
        Email.UpdateEmail(newEmail);
        SetUpdatedAt();
    }

    public void UpdateName(string? newName)
    {
        UserBaseGuard.AgainstNullOrEmptyName(newName);
        Name = newName;
        SetUpdatedAt();
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        SetUpdatedAt();
    }
}
