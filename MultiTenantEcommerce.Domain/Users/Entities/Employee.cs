using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Users.Entities;
public class Employee : UserBase
{
    private readonly HashSet<EmployeeRole> _employeeRoles = new();
    public IReadOnlyCollection<EmployeeRole> EmployeeRoles => _employeeRoles;

    private Employee() { }
    public Employee(Guid tenantId, string name, Email email, Password password)
        : base(tenantId, name, password, email)
    {
    }
    public void AddRole(Role role)
    {
        if (_employeeRoles.Any(x => x.RoleId == role.Id))
            return;

        _employeeRoles.Add(new EmployeeRole(Id, role.Id));
    }

    public void RemoveRole(Role role)
    {
        var employeeRole = _employeeRoles.FirstOrDefault(x => x.RoleId == role.Id);

        if (employeeRole != null)
            _employeeRoles.Remove(employeeRole);
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
    public void UpdateRole(string role)
    {
        //TODO()
        SetUpdatedAt();
    }
    #endregion
}
