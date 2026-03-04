using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Queries.GetFiltered;

public class
    GetFilteredTenantMembersQueryHandler : IQueryHandler<GetFilteredTenantMembersQuery, List<TenantMemberResponseDTO>>
{
    private readonly TenantMemberMapper _employeeMapper;
    private readonly ITenantMemberRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;

    public GetFilteredTenantMembersQueryHandler(ITenantMemberRepository employeeRepository,
        TenantMemberMapper employeeMapper,
        IRoleRepository roleRepository)
    {
        _employeeRepository = employeeRepository;
        _employeeMapper = employeeMapper;
        _roleRepository = roleRepository;
    }

    public async Task<List<TenantMemberResponseDTO>> Handle(GetFilteredTenantMembersQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetFilteredAsync(
            request.Name,
            request.Role,
            request.Email,
            request.IsActive,
            request.Page,
            request.PageSize,
            request.Sort);

        var allRoleIds = employees
            .SelectMany(x => x.TenantMemberRoles)
            .Select(x => x.RoleId)
            .Distinct()
            .ToList();

        var roles = await _roleRepository.GetByIdsAsync(allRoleIds);

        return _employeeMapper.ToTenantMemberResponseDTOList(employees, roles);
    }
}