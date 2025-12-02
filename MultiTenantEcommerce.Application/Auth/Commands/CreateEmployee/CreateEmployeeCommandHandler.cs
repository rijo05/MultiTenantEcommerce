using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Helpers.Services;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Application.Users.Employees.Mappers;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Auth.Commands.CreateEmployee;
public class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, AuthEmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly ITenantContext _tenantContext;
    private readonly EmployeeMapper _employeeMapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PasswordGenerator _passwordGenerator;
    private readonly ITokenService _tokenService;
    private readonly IRoleRepository _roleRepository;

    public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository,
        IPermissionRepository permissionRepository,
        ITenantContext tenantContext,
        EmployeeMapper employeeMapper,
        IUnitOfWork unitOfWork,
        PasswordGenerator passwordGenerator,
        ITokenService tokenService,
        IRoleRepository roleRepository)
    {
        _employeeRepository = employeeRepository;
        _permissionRepository = permissionRepository;
        _tenantContext = tenantContext;
        _employeeMapper = employeeMapper;
        _unitOfWork = unitOfWork;
        _passwordGenerator = passwordGenerator;
        _tokenService = tokenService;
        _roleRepository = roleRepository;
    }

    public async Task<AuthEmployeeResponseDTO> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var existingEmployee = await _employeeRepository.GetByEmail(new Email(request.Email));
        if (existingEmployee is not null)
            throw new Exception("Employee with this email already exists.");

        var roles = await _roleRepository.GetRolesWithPermissionsByIdsAsync(request.RolesId);

        if (roles.Count != request.RolesId.Count)
            throw new Exception("One or more roles do not exist.");

        var permissionIds = roles
            .SelectMany(r => r.Permissions)
            .Select(rp => rp.PermissionId)
            .Distinct()
            .ToList();

        var permissionNames = await _permissionRepository.GetPermissionNamesByIdsAsync(permissionIds);
        var roleNames = roles.Select(r => r.Name).ToList();

        var randomPassword = _passwordGenerator.GenerateRandomPassword();

        var employee = new Employee(
            _tenantContext.TenantId,
            request.Name,
            new Email(request.Email),
            new Password(randomPassword),
            request.RolesId);

        await _employeeRepository.AddAsync(employee);
        await _unitOfWork.CommitAsync();

        return new AuthEmployeeResponseDTO
        {
            Id = employee.Id,
            Email = employee.Email.Value,
            Name = employee.Name,
            Permissions = permissionNames,
            Roles = roleNames,
            Token = _tokenService.GenerateToken(employee, roleNames, permissionNames)
        };
    }
}
