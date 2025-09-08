using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetByRole;
public class GetEmployeesByRolesQueryHandler : IQueryHandler<GetEmployeesByRolesQuery, List<EmployeeResponseDTO>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly EmployeeMapper _employeeMapper;

    public GetEmployeesByRolesQueryHandler(IEmployeeRepository employeeRepository, EmployeeMapper employeeMapper)
    {
        _employeeRepository = employeeRepository;
        _employeeMapper = employeeMapper;
    }

    public async Task<List<EmployeeResponseDTO>> Handle(GetEmployeesByRolesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetEmployeesByRole(request.roleId);

        return _employeeMapper.ToEmployeeResponseDTOList(employees);
    }
}
