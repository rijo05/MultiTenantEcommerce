using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetById;
public class GetEmployeeByIdQueryHandler : IQueryHandler<GetEmployeeByIdQuery, EmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository; 
    private readonly EmployeeMapper _employeeMapper;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository,
        IRoleRepository roleRepository,
        EmployeeMapper employeeMapper)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _employeeMapper = employeeMapper;
    }

    public async Task<EmployeeResponseDTO> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId)
            ?? throw new Exception("Employee doesnt exist.");

        var roles = await _roleRepository.GetByIdsAsync(employee.EmployeeRoles.Select(x => x.Id).ToList());

        return _employeeMapper.ToEmployeeResponseDTO(employee, roles);
    }
}
