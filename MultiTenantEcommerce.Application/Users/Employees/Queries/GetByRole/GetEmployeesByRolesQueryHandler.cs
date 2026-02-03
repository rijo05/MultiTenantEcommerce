using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetByRole;
public class GetEmployeesByRolesQueryHandler : IQueryHandler<GetEmployeesByRolesQuery, List<EmployeeResponseDTO>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly EmployeeMapper _employeeMapper;

    public GetEmployeesByRolesQueryHandler(IEmployeeRepository employeeRepository,
        IRoleRepository roleRepository,
        EmployeeMapper employeeMapper)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _employeeMapper = employeeMapper;
    }

    public async Task<List<EmployeeResponseDTO>> Handle(GetEmployeesByRolesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetEmployeesByRole(
            request.roleId,
            request.Page,
            request.PageSize,
            request.Sort);

        var allRoleIds = employees.SelectMany(x => x.EmployeeRoles).Select(x => x.RoleId).ToList();

        var roles = await _roleRepository.GetByIdsAsync(allRoleIds);

        return _employeeMapper.ToEmployeeResponseDTOList(employees, roles);
    }
}
