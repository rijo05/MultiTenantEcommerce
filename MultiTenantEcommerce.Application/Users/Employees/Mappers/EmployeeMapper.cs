using MultiTenantEcommerce.Application.Common.Helpers.Services;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Entities;

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

    public EmployeeResponseDTO ToEmployeeResponseDTO(Employee employee)
    {
        var roles = employee.EmployeeRoles.Select(x => x.Role).ToList();
        var permissions = roles.SelectMany(r => r.Permissions).DistinctBy(p => p.Id);

        return new EmployeeResponseDTO
        {
            Id = employee.Id,
            Name = employee.Name,
            Email = employee.Email.Value,
            Roles = _rolesMapper.ToRoleResponseDTOList(roles),
            Permissions = _rolesMapper.ToPermissionResponseDTOList(permissions),
            IsActive = employee.IsActive,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt,
            //Links = GenerateLinks(Employee)
        };
    }

    public List<EmployeeResponseDTO> ToEmployeeResponseDTOList(IEnumerable<Employee> Employees)
    {
        return Employees.Select(x => ToEmployeeResponseDTO(x)).ToList();
    }


    //private Dictionary<string, object> GenerateLinks(Employee Employee)
    //{
    //    return _hateoasLinkService.GenerateLinksCRUD(
    //                Employee.Id,
    //                "Employees",
    //                "GetById",
    //                "Update",
    //                "Delete"
    //    );
    //}
}
