using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Users.Employees.DTOs;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Employees.Commands.AssignRole;
public class AssignRoleToEmployeeCommandHandler : ICommandHandler<AssignRoleToEmployeeCommand, EmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly EmployeeMapper _employeeMapper;

    public AssignRoleToEmployeeCommandHandler(IEmployeeRepository employeeRepository,
        IRoleRepository roleRepository,
        IUnitOfWork unitOfWork,
        EmployeeMapper employeeMapper)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _employeeMapper = employeeMapper;
    }

    public async Task<EmployeeResponseDTO> Handle(AssignRoleToEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAllIncluded(request.employeeId)
            ?? throw new Exception("Employee doesnt exist.");

        var roles = await _roleRepository.GetByIdsAsync(request.roles.Distinct());

        var missingIds = request.roles.Distinct().Except(roles.Select(r => r.Id)).ToList();
        if (missingIds.Any())
            throw new Exception($"Invalid role ids: {string.Join(", ", missingIds)}");

        foreach (var item in roles)
            employee.AddRole(item);

        await _unitOfWork.CommitAsync();

        return _employeeMapper.ToEmployeeResponseDTO(employee);
    }
}
