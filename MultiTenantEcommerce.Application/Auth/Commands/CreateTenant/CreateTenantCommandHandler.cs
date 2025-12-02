using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Auth.Commands.CreateTenant;
public class CreateTenantCommandHandler : ICommandHandler<CreateTenantCommand, AuthTenantResponse>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITokenService _tokenService;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;

    public CreateTenantCommandHandler(ITenantRepository tenantRepository,
        IUnitOfWork unitOfWork,
        IEmployeeRepository employeeRepository,
        ITokenService tokenService,
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _employeeRepository = employeeRepository;
        _tokenService = tokenService;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
    }

    public async Task<AuthTenantResponse> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var existingTenant = await _tenantRepository.GetByCompanyNameAllIncluded(request.CompanyName);

        if (existingTenant is not null)
            throw new Exception("Company with this name already exists.");

        var tenant = new Tenant(request.CompanyName, new Email(request.CompanyEmail));
        await _tenantRepository.AddAsync(tenant);

        var ownerRole = new Role(tenant.Id, SystemRoles.Owner, "Full access");
        var adminRole = new Role(tenant.Id, SystemRoles.Admin, "Full access except tenant settings");

        var allPermissions = await _permissionRepository.GetAllAsync();

        foreach (var perm in allPermissions)
        {
            ownerRole.AddPermission(perm.Id);

            if (!perm.Area.Equals("tenant", StringComparison.OrdinalIgnoreCase))
            {
                adminRole.AddPermission(perm.Id);
            }
        }

        ownerRole.MarkRoleAsSystemRole();
        adminRole.MarkRoleAsSystemRole();

        await _roleRepository.AddAsync(ownerRole);
        await _roleRepository.AddAsync(adminRole);


        var employee = new Employee(
            tenant.Id,
            request.OwnerName,
            new Email(request.OwnerEmail),
            new Password(request.Password), new List<Guid> { ownerRole.Id });

        await _employeeRepository.AddAsync(employee);

        await _unitOfWork.CommitAsync();

        var roleNames = new List<string> { ownerRole.Name };
        var permissionNames = allPermissions.Select(p => p.Name).ToList();

        return new AuthTenantResponse
        {
            Email = employee.Email.Value,
            OwnerId = employee.Id,
            Name = employee.Name,
            Token = _tokenService.GenerateToken(employee, roleNames, permissionNames)
        };

        //por agora q ira haver apenas 1 plano gratis, no futuro vejo como fazer planos pagos
    }
}
