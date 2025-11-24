using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetById;
public class GetEmployeeByIdQueryHandler : IQueryHandler<GetEmployeeByIdQuery, EmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly EmployeeMapper _employeeMapper;

    public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository,
        EmployeeMapper employeeMapper)
    {
        _employeeRepository = employeeRepository;
        _employeeMapper = employeeMapper;
    }

    public async Task<EmployeeResponseDTO> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAllIncluded(request.EmployeeId)
            ?? throw new Exception("Employee doesnt exist.");

        return _employeeMapper.ToEmployeeResponseDTO(employee);
    }
}
