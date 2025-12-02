using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetByEmail;
public class GetEmployeeByEmailQueryHandler : IQueryHandler<GetEmployeeByEmailQuery, EmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly EmployeeMapper _employeeMapper;

    public GetEmployeeByEmailQueryHandler(IEmployeeRepository employeeRepository,
        IRoleRepository roleRepository,
        EmployeeMapper employeeMapper)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _employeeMapper = employeeMapper;
    }

    public async Task<EmployeeResponseDTO> Handle(GetEmployeeByEmailQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByEmail(new Email(request.Email))
            ?? throw new Exception("Employee with that email doesnt exist.");

        var roles = await _roleRepository.GetByIdsAsync(employee.EmployeeRoles.Select(x => x.Id).ToList());

        return _employeeMapper.ToEmployeeResponseDTO(employee, roles);
    }
}
