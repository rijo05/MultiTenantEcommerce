using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetByEmail;
public class GetEmployeeByEmailQueryHandler : IQueryHandler<GetEmployeeByEmailQuery, EmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly EmployeeMapper _employeeMapper;

    public GetEmployeeByEmailQueryHandler(IEmployeeRepository employeeRepository,
        EmployeeMapper employeeMapper)
    {
        _employeeRepository = employeeRepository;
        _employeeMapper = employeeMapper;
    }

    public async Task<EmployeeResponseDTO> Handle(GetEmployeeByEmailQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByEmailAsync(new Email(request.Email)) 
            ?? throw new Exception("Employee with that email doesnt exist.");

        return _employeeMapper.ToEmployeeResponseDTO(employee);
    }
}
