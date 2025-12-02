using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetFiltered;
public class GetFilteredEmployeesQueryHandler : IQueryHandler<GetFilteredEmployeesQuery, List<EmployeeResponseDTO>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly EmployeeMapper _employeeMapper;

    public GetFilteredEmployeesQueryHandler(IEmployeeRepository employeeRepository, 
        EmployeeMapper employeeMapper,
        IRoleRepository roleRepository)
    {
        _employeeRepository = employeeRepository;
        _employeeMapper = employeeMapper;
        _roleRepository = roleRepository;
    }

    public async Task<List<EmployeeResponseDTO>> Handle(GetFilteredEmployeesQuery    request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetFilteredAsync(
            request.Name,
            request.Role,
            request.Email,
            request.IsActive,
            request.Page,
            request.PageSize,
            request.Sort);

        var allRoleIds = employees
            .SelectMany(x => x.EmployeeRoles)
            .Select(x => x.RoleId)
            .Distinct()
            .ToList();

        var roles = await _roleRepository.GetByIdsAsync(allRoleIds);

        return _employeeMapper.ToEmployeeResponseDTOList(employees, roles);
    }
}
