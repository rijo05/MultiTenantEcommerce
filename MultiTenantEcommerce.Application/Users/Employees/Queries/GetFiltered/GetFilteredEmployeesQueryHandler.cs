using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Users.Employees.Queries.GetFiltered;
public class GetFilteredEmployeesQueryHandler : IQueryHandler<GetFilteredEmployeesQuery, List<EmployeeResponseDTO>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly EmployeeMapper _employeeMapper;

    public GetFilteredEmployeesQueryHandler(IEmployeeRepository employeeRepository, EmployeeMapper employeeMapper)
    {
        _employeeRepository = employeeRepository;
        _employeeMapper = employeeMapper;
    }

    public async Task<List<EmployeeResponseDTO>> Handle(GetFilteredEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetFilteredAsync(
            request.Name,
            request.Role,
            request.Email,
            request.IsActive,
            request.Page,
            request.PageSize,
            request.Sort);

        return _employeeMapper.ToEmployeeResponseDTOList(employees);
    }
}
