using MediatR;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Services;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Domain.Platform.Tenancy.Services;
public class TenantMemberPolicy : ITenantMemberPolicy
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly ITenantMemberRepository _tenantMemberRepository;

    public TenantMemberPolicy(IInvitationRepository invitationRepository, 
        ITenantMemberRepository tenantMemberRepository)
    {
        _invitationRepository = invitationRepository;
        _tenantMemberRepository = tenantMemberRepository;
    }

    public async Task EnsureCanAddMemberAsync(Guid? userId, Email email, int maxMembers)
    {
        var pendingInvite = await _invitationRepository.GetPendingByEmail(email);
        if (pendingInvite != null)
            throw new Exception("This email already has a pending invitation");

        if (await _tenantMemberRepository.CountMembers() >= maxMembers)
            throw new Exception("Max members have been reached");

        if (userId.HasValue)
        {
            var isMember = await _tenantMemberRepository.IsMemberAsync(userId.Value);

            if (isMember)
                throw new Exception("This user already belongs to the team");
        }
    }
}
