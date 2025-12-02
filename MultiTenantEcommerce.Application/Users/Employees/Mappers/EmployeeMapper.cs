using MultiTenantEcommerce.Application.Common.Helpers.Services;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;

namespace MultiTenantEcommerce.Application.Users.Employees.Mappers;

public class EmployeeMapper
{
    private readonly HateoasLinkService _hateoasLinkService;
    private readonly RolesMapper _rolesMapper;

    public EmployeeMapper(HateoasLinkService hateoasLinkService,
        RolesMapper rolesMapper)
    {
        _hateoasLinkService = hateoasLinkService;
        _rolesMapper = rolesMapper;
    }

    public EmployeeResponseDTO ToEmployeeResponseDTO(Employee employee, List<Role> roles)
    {
        return new EmployeeResponseDTO
        {
            Id = employee.Id,
            Name = employee.Name,
            Email = employee.Email.Value,
            Roles = _rolesMapper.ToRoleResponseDTOList(roles),
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt,
        };
    }

    public List<EmployeeResponseDTO> ToEmployeeResponseDTOList(List<Employee> employees, List<Role> allRoles)
    {
        var roleDict = allRoles.ToDictionary(r => r.Id);

        return employees.Select(emp =>
        {
            var empRoleIds = emp.EmployeeRoles.Select(er => er.RoleId).ToList();

            var empRoles = empRoleIds
                .Select(id => roleDict.TryGetValue(id, out var r) ? r : null)
                .Where(r => r != null)
                .ToList()!;

            return ToEmployeeResponseDTO(emp, empRoles);
        }).ToList();
    }
}
