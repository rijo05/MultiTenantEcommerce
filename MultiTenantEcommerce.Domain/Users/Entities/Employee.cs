using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.Users.Events;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Users.Entities;
public class Employee : UserBase
{
    private readonly HashSet<EmployeeRole> _employeeRoles = new();
    public IReadOnlyCollection<EmployeeRole> EmployeeRoles => _employeeRoles;

    private Employee() { }
    public Employee(Guid tenantId, string name, Email email, Password password, List<Role>? roles)
        : base(tenantId, name, password, email)
    {
        roles?.ForEach(x => AddRole(x));

        AddDomainEvent(new EmployeeRegisteredEvent(this.TenantId, this.Id));
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
