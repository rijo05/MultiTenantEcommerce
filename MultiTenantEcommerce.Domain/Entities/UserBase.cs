using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Entities;
public abstract class UserBase
{
    public Guid Id { get; protected set; }
    public Guid TenantId { get; protected set; }
    public string Name { get; protected set; }
    public Email Email { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }

    protected UserBase() { }

    protected UserBase(Guid tenantId, string name, Email email)
    {
        UserBaseGuard.AgainstNullOrEmptyName(name);
        UserBaseGuard.AgainstInvalidEmail(email.Value);


        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void UpdateName(string? newName)
    {
        UserBaseGuard.AgainstNullOrEmptyName(newName);
        Name = newName;
        UpdatedAt = DateTime.UtcNow; ;
    }

    public void UpdateEmail(string? newEmail)
    {
        Email.UpdateEmail(newEmail);
        UpdatedAt = DateTime.UtcNow;
    }
}
