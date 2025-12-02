using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.Users.Events;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Users.Entities;
public class Employee : UserBase
{
    private readonly HashSet<EmployeeRole> _employeeRoles = new();
    public IReadOnlyCollection<EmployeeRole> EmployeeRoles => _employeeRoles;

    private Employee() { }
    public Employee(Guid tenantId, string name, Email email, Password password, List<Guid>? roleIds)
        : base(tenantId, name, password, email)
    {
        if (roleIds != null)
        {
            foreach (var item in roleIds)
            {
                AddRole(item);
            }
        }

        AddDomainEvent(new EmployeeRegisteredEvent(this.TenantId, this.Id));
    }
    public void AddRole(Guid roleId)
    {
        if (_employeeRoles.Any(x => x.RoleId == roleId))
            return;

        _employeeRoles.Add(new EmployeeRole(this.TenantId, Id, roleId));
    }

    public void RemoveRole(Guid roleId)
    {
        var employeeRole = _employeeRoles.FirstOrDefault(x => x.RoleId == roleId);

        if (employeeRole != null)
            _employeeRoles.Remove(employeeRole);
    }

    #region UPDATE DATA

    public void UpdateEmployee(
        string? name,
        string? email,
        string? password,
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
    }
    #endregion
}
