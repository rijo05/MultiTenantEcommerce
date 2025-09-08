using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Users.Permissions.DTOs;
using MultiTenantEcommerce.Application.Users.Permissions.Mappers;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;

namespace MultiTenantEcommerce.Application.Users.Permissions.Commands.AddToRole;
public class AddPermissionsToRoleCommandHandler : ICommandHandler<AddPermissionsToRoleCommand, RoleResponseDTO>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly RolesMapper _rolesMapper;
    private readonly IUnitOfWork _unitOfWork;

    public AddPermissionsToRoleCommandHandler(IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        RolesMapper rolesMapper,
        IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _rolesMapper = rolesMapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<RoleResponseDTO> Handle(AddPermissionsToRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetRoleAndPermissionById(request.roleId)
            ?? throw new Exception("Role doesnt exist.");

        var permissions = await _permissionRepository.GetByIdsAsync(request.permissions);

        var missingIds = request.permissions.Except(permissions.Select(p => p.Id)).ToList();
        if (missingIds.Any())
            throw new Exception($"Invalid permission ids: {string.Join(",", missingIds)}");

        foreach (var item in permissions)
        {
            role.AddPermission(item);
        }

        await _unitOfWork.CommitAsync();

        return _rolesMapper.ToRoleResponseDTO(role);
    }
}
