using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand, RoleDetailResponseDTO>
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly RolesMapper _rolesMapper;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleCommandHandler(IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        ITenantContext tenantContext,
        RolesMapper rolesMapper,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _tenantContext = tenantContext;
        _rolesMapper = rolesMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleDetailResponseDTO> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        if (await _roleRepository.GetByNameAsync(request.Name) is not null)
            throw new Exception("Role with that name already exists");

        var permissions = await _permissionRepository.GetByIdsAsync(request.Permissions);

        if (permissions.Count != request.Permissions.Distinct().Count())
            throw new Exception("Some ids are not valid");

        var role = new Role(_tenantContext.TenantId, request.Name, request.Description);

        foreach (var item in permissions) role.AddPermission(item.Id);

        await _roleRepository.AddAsync(role);

        await _unitOfWork.CommitAsync();

        return _rolesMapper.ToRoleDetailResponseDTO(role, permissions);
    }
}