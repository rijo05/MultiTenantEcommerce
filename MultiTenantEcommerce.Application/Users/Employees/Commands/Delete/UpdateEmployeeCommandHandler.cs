using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Common.Validators;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Users.Employees.Commands.Delete;
public class UpdateEmployeeCommandHandler : ICommandHandler<UpdateEmployeeCommand, EmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly TenantContext _tenantContext;
    private readonly EmployeeMapper _employeeMapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository,
        TenantContext tenantContext,
        EmployeeMapper employeeMapper,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _tenantContext = tenantContext;
        _employeeMapper = employeeMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeResponseDTO> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id) 
            ?? throw new Exception("Employee doesnt exist.");


        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var existingEmail = await _employeeRepository.GetByEmailAsync(new Email(request.Email));
            if (existingEmail is not null && existingEmail.Id != employee.Id)
                throw new Exception("Email already in use.");
        }

        employee.UpdateEmployee(
            request.Name,
            request.Email,
            request.Password,
            request.Role,
            request.IsActive);


        await _unitOfWork.CommitAsync();
        return _employeeMapper.ToEmployeeResponseDTO(employee);
    }
}
