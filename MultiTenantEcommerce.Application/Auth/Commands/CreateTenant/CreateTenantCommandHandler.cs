using MultiTenantEcommerce.Application.Auth.DTOs;
using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
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
        var existingTenant = await _tenantRepository.GetByCompanyName(request.CompanyName);

        if (existingTenant is not null)
            throw new Exception("Company with this name already exists.");

        var tenant = new Tenant(request.CompanyName, new Email(request.CompanyEmail));


        var ownerRole = new Role("Owner", "Has all permissions", tenant.Id);
        var adminRole = new Role("Admin", "Has all permissions except tenant related ones", tenant.Id);

        var permissions = await _permissionRepository.GetAllAsync();

        permissions.ToList()
            .ForEach(x => ownerRole.AddPermission(x));

        permissions.Where(p => p.Area != "tenant").ToList()
            .ForEach(x => adminRole.AddPermission(x));

        ownerRole.MarkRoleAsSystemRole();
        adminRole.MarkRoleAsSystemRole();


        var employee = new Employee(
            tenant.Id,
            request.OwnerName,
            new Email(request.OwnerEmail),
            new Password(request.Password), new List<Role> { ownerRole });


        await _tenantRepository.AddAsync(tenant);
        await _employeeRepository.AddAsync(employee);
        await _roleRepository.AddAsync(ownerRole);
        await _roleRepository.AddAsync(adminRole);

        await _unitOfWork.CommitAsync();

        return new AuthTenantResponse
        {
            Email = employee.Email.Value,
            OwnerId = employee.Id,
            Name = employee.Name,
            Token = _tokenService.CreateToken(employee)
        };

        //por agora q ira haver apenas 1 plano gratis, no futuro vejo como fazer planos pagos
    }
}
