using MultiTenantEcommerce.Domain.Common.Guard;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Users.Entities;
public class Employee : UserBase
{
    public Role Role { get; private set; }
    public Password Password { get; private set; }

    private Employee() { }
    public Employee(Guid tenantId, string name, Email email, Role role, Password password) 
        : base(tenantId, name, email)
    {
        Role = role;
        Password = password;
    }

    #region UPDATE DATA

    public void UpdateEmployee(
        string? name,
        string? email,
        string? password,
        string? role,
        bool? isActive)
    {
        if (!string.IsNullOrWhiteSpace(name))
            UpdateName(name);

        if (!string.IsNullOrWhiteSpace(email))
            UpdateEmail(email);

        if (!string.IsNullOrWhiteSpace(password))
            UpdatePassword(password);

        if (isActive.HasValue)
            SetActive(isActive.Value);

        if (!string.IsNullOrWhiteSpace(role))
            UpdateRole(role);
    }
    public void UpdatePassword(string newPassword)
    {
        Password.UpdatePassword(newPassword);
        SetUpdatedAt();
    }

    public void UpdateRole(string role)
    {
        //TODO()
        SetUpdatedAt();
    }
    #endregion
}
