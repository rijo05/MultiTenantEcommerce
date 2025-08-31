using MediatR;
using MultiTenantEcommerce.Domain.Common.Entities;
using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Users.Entities;
public abstract class UserBase : TenantBase
{
    public string Name { get; protected set; }
    public Email Email { get; protected set; }
    public bool IsActive { get; protected set; }

    protected UserBase() { }

    protected UserBase(Guid tenantId, string name, Email email) 
        : base(tenantId)
    {
        UserBaseGuard.AgainstNullOrEmptyName(name);
        UserBaseGuard.AgainstInvalidEmail(email.Value);

        Name = name;
        Email = email;
        IsActive = true;
    }


    public void UpdateName(string? newName)
    {
        UserBaseGuard.AgainstNullOrEmptyName(newName);
        Name = newName;
        SetUpdatedAt();
    }

    public void UpdateEmail(string? newEmail)
    {
        Email.UpdateEmail(newEmail);
        SetUpdatedAt();
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        SetUpdatedAt();
    }
}
