using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Shared.Application.CQRS;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Queries.GetMyWorkspaces;
public class GetMyWorkspacesQueryHandler : IQueryHandler<GetMyWorkspacesQuery, List<UserWorkspaceDTO>>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantMemberRepository _tenantMemberRepository;

    public GetMyWorkspacesQueryHandler(ITenantRepository tenantRepository, ITenantMemberRepository tenantMemberRepository)
    {
        _tenantRepository = tenantRepository;
        _tenantMemberRepository = tenantMemberRepository;
    }

    public async Task<List<UserWorkspaceDTO>> Handle(GetMyWorkspacesQuery request, CancellationToken cancellationToken)
    {
        if (request.IsPlatformAdmin)
            throw new Exception("platform admin shouldnt talk to this endpoint");

        var tenants = await _tenantRepository.GetTenantsForUserAsync(request.UserId);

        var member = await _tenantMemberRepository.GetMemberByUserId(request.UserId);

        var workspaceList = new List<UserWorkspaceDTO>();

        foreach (var tenant in tenants)
        {
            var membership = member.First(m => m.TenantId == tenant.Id);

            workspaceList.Add(new UserWorkspaceDTO(
                tenant.Id,
                tenant.Name,
                tenant.SubDomain,
                membership.IsOwner));
        }

        return workspaceList;
    }
}
