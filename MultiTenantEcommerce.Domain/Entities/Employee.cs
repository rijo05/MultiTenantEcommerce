using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Entities;
public class Employee : UserBase
{
    public Role Role { get; private set; }
    public Password Password { get; private set; }

    private Employee() { }
    public Employee(Guid tenantId, string name, Email email, Role role, Password password) : base(tenantId, name, email)
    {
        Role = role;
        Password = password;
    }

    #region UPDATE DATA

    public void UpdatePassword(string newPassword)
    {
        Password.UpdatePassword(newPassword);
        SetUpdatedAt();
    }

    public void UpdateRole(Role role)
    {
        //TODO()
        SetUpdatedAt();
    }
    #endregion
}
