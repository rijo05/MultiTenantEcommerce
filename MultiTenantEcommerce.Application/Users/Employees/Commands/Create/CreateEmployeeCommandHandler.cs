using FluentValidation;
using MultiTenantEcommerce.Application.Common.Interfaces;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Application.Users.Employees.Commands.Create;
public class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, EmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly TenantContext _tenantContext;
    private readonly EmployeeMapper _employeeMapper;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository,
        TenantContext tenantContext,
        EmployeeMapper employeeMapper,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _tenantContext = tenantContext;
        _employeeMapper = employeeMapper;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<EmployeeResponseDTO> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var existingEmployee = await _employeeRepository.GetByEmailAsync(new Email(request.Email));
        if (existingEmployee is not null)
            throw new Exception("Employee with this email already exists.");


        var Employee = new Domain.Users.Entities.Employee(
            _tenantContext.TenantId,
            request.Name,
            new Email(request.Email),
            new Role(request.Role),
            new Password(request.Password));


        await _employeeRepository.AddAsync(Employee);
        await _unitOfWork.CommitAsync();
        return _employeeMapper.ToEmployeeResponseDTO(Employee);
    }
}
