using MediatR;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Services;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using MultiTenantEcommerce.Shared.Integration.Proxies;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.Commands.InviteMember;
public class InviteMemberCommandHandler : ICommandHandler<InviteMemberCommand, Unit>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IIdentityIntegrationProxy _identityIntegrationProxy;
    private readonly ITenantContext _tenantContext;
    private readonly IBillingIntegrationProxy _billingIntegrationProxy;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantRepository _tenantRepository;
    private readonly ITenantMemberPolicy _tenantMemberPolicy;

    public InviteMemberCommandHandler(IInvitationRepository invitationRepository, 
        IRoleRepository roleRepository, 
        IIdentityIntegrationProxy identityIntegrationProxy, 
        ITenantContext tenantContext, 
        IBillingIntegrationProxy billingIntegrationProxy, 
        IUnitOfWork unitOfWork, 
        ITenantRepository tenantRepository, 
        ITenantMemberPolicy tenantMemberPolicy)
    {
        _invitationRepository = invitationRepository;
        _roleRepository = roleRepository;
        _identityIntegrationProxy = identityIntegrationProxy;
        _tenantContext = tenantContext;
        _billingIntegrationProxy = billingIntegrationProxy;
        _unitOfWork = unitOfWork;
        _tenantRepository = tenantRepository;
        _tenantMemberPolicy = tenantMemberPolicy;
    }

    public async Task<Unit> Handle(InviteMemberCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _tenantRepository.GetByIdAsync(_tenantContext.TenantId) 
            ?? throw new Exception("tenant doesnt exist");

        var plan = await _billingIntegrationProxy.GetPlanLimitsAsync(tenant.Subscription.PlanId) 
            ?? throw new Exception("Couldnt get plan");

        var existingUserId = await _identityIntegrationProxy.GetUserIdByEmailAsync(request.Email);

        await _tenantMemberPolicy.EnsureCanAddMemberAsync(existingUserId, new Email(request.Email), plan.MaxTeamMembers);

        var roles = await _roleRepository.GetByIdsAsync(request.RolesId.Distinct());
        if (roles.Count != request.RolesId.Distinct().Count())
            throw new Exception("Roles are invalid");


        var invitation = new TenantInvitation(_tenantContext.TenantId, new(request.Email), roles.Select(x => x.Id).Distinct().ToList());

        await _invitationRepository.AddAsync(invitation);
        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}
