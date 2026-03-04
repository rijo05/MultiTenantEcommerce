using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.DTOs;
using MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Common.Mappers;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Queries.GetById;

public class GetTenantMemberByIdQueryHandler : IQueryHandler<GetTenantMemberByIdQuery, TenantMemberResponseDTO>
{
    private readonly TenantMemberMapper _employeeMapper;
    private readonly ITenantMemberRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;

    public GetTenantMemberByIdQueryHandler(ITenantMemberRepository employeeRepository,
        IRoleRepository roleRepository,
        TenantMemberMapper employeeMapper)
    {
        _employeeRepository = employeeRepository;
        _roleRepository = roleRepository;
        _employeeMapper = employeeMapper;
    }

    public async Task<TenantMemberResponseDTO> Handle(GetTenantMemberByIdQuery request,
        CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(request.TenantMemberId)
                       ?? throw new Exception("TenantMember doesnt exist.");

        var roles = await _roleRepository.GetByIdsAsync(employee.TenantMemberRoles.Select(x => x.Id).ToList());

        return _employeeMapper.ToTenantMemberResponseDTO(employee, roles);
    }
}