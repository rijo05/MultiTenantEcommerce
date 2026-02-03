using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Auth.Queries.LoginEmployee;
public class LoginEmployeeQueryHandler : IQueryHandler<LoginEmployeeQuery, AuthEmployeeResponseDTO>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly ITokenService _tokenService;

    public LoginEmployeeQueryHandler(IEmployeeRepository employeeRepository,
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        ITokenService tokenService)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthEmployeeResponseDTO> Handle(LoginEmployeeQuery request, CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByEmail(new Email(request.Email));

        if (employee is null || !employee.Password.VerifySamePassword(request.Password))
            throw new Exception("Email or password are wrong");

        var roles = await _roleRepository.GetRolesWithPermissionsByIdsAsync(employee.EmployeeRoles.Select(x => x.RoleId).ToList());
        var roleNames = roles.Select(x => x.Name).Distinct().ToList();

        var permissionIds = roles
                .SelectMany(x => x.Permissions)
                .Select(x => x.PermissionId)
                .Distinct();

        var permissions = await _permissionRepository.GetPermissionNamesByIdsAsync(permissionIds);

        return new AuthEmployeeResponseDTO
        {
            Email = request.Email,
            Id = employee.Id,
            Name = employee.Name,
            Permissions = permissions,
            Roles = roleNames,
            Token = _tokenService.GenerateToken(employee, roleNames, permissions)
        };
    }
}
