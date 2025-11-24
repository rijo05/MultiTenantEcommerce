using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Users.Employees.Commands.Update;
public class UpdateEmployeeCommandHandler : ICommandHandler<UpdateEmployeeCommand, EmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly EmployeeMapper _employeeMapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository,
        EmployeeMapper employeeMapper,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _employeeMapper = employeeMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeResponseDTO> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id)
            ?? throw new Exception("Employee doesnt exist.");


        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var existingEmail = await _employeeRepository.GetByEmailAllIncluded(new Email(request.Email));
            if (existingEmail is not null && existingEmail.Id != employee.Id)
                throw new Exception("Email already in use.");
        }

        employee.UpdateEmployee(
            request.Name,
            request.Email,
            request.Password,
            request.IsActive);


        await _unitOfWork.CommitAsync();
        return _employeeMapper.ToEmployeeResponseDTO(employee);
    }
}
