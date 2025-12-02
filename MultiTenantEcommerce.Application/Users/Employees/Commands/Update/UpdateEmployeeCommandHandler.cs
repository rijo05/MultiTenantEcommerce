using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Users.Employees.Commands.Update;
public class UpdateEmployeeCommandHandler : ICommandHandler<UpdateEmployeeCommand, EmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly EmployeeMapper _employeeMapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository,
        IRoleRepository roleRepository,
        EmployeeMapper employeeMapper,
        IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _employeeMapper = employeeMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeResponseDTO> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.Id)
            ?? throw new Exception("Employee doesnt exist.");

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var existingEmail = await _employeeRepository.GetByEmail(new Email(request.Email));
            if (existingEmail is not null && existingEmail.Id != employee.Id)
                throw new Exception("Email already in use.");
        }

        employee.UpdateEmployee(
            request.Name,
            request.Email,
            request.Password,
            request.IsActive);

        var roleIds = employee.EmployeeRoles.Select(x => x.RoleId).ToList();

        var roles = roleIds.Any()
            ? await _roleRepository.GetByIdsAsync(roleIds)
            : new List<Role>();

        await _unitOfWork.CommitAsync();
        return _employeeMapper.ToEmployeeResponseDTO(employee, roles);
    }
}
