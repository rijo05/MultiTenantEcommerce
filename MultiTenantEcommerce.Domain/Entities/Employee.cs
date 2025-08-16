using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Entities;
public class Employee : UserBase
{
    public Role Role { get; private set; }
    public Password Password { get; private set; }
    public bool IsActive { get; private set; }

    private Employee() { }

    public Employee(Guid tenantId, string name, Email email, Role role, Password password) : base(tenantId, name, email)
    {
        Role = role;
        Password = password;
        IsActive = true;
    }

    #region UPDATE DATA

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow; ;
    }

    public void UpdatePassword(string newPassword)
    {
        Password.UpdatePassword(newPassword);
        UpdatedAt = DateTime.UtcNow;
    }

    #endregion

    #region PRIVATES
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    #endregion
}
