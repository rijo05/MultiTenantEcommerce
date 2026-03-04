using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Queries.GetByRole;

public class
    GetTenantMembersByRolesQueryHandler : IQueryHandler<GetTenantMembersByRolesQuery, List<TenantMemberResponseDTO>>
{
    private readonly TenantMemberMapper _employeeMapper;
    private readonly ITenantMemberRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;

    public GetTenantMembersByRolesQueryHandler(ITenantMemberRepository employeeRepository,
        IRoleRepository roleRepository,
        TenantMemberMapper employeeMapper)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _employeeMapper = employeeMapper;
    }

    public async Task<List<TenantMemberResponseDTO>> Handle(GetTenantMembersByRolesQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetTenantMembersByRole(
            request.roleId,
            request.Page,
            request.PageSize,
            request.Sort);

        var allRoleIds = employees.SelectMany(x => x.TenantMemberRoles).Select(x => x.RoleId).ToList();

        var roles = await _roleRepository.GetByIdsAsync(allRoleIds);

        return _employeeMapper.ToTenantMemberResponseDTOList(employees, roles);
    }
}