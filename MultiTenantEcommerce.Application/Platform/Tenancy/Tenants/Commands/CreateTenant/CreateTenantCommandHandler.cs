using MediatR;
using MultiTenantEcommerce.Domain.Platform.Billing.Interfaces;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Entities.Auth;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Services;
using MultiTenantEcommerce.Shared.Application.CQRS;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;
using MultiTenantEcommerce.Shared.Integration.Proxies;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.Tenants.Commands.CreateTenant;

public class CreateTenantCommandHandler : ICommandHandler<CreateTenantCommand, Unit>
{
    private readonly ITenantMemberRepository _tenantMemberRepository;
    private readonly IBillingIntegrationProxy _billingIntegrationProxy;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPermissionRepository _permissionRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITenantCreationPolicy _tenantCreationPolicy;

    public CreateTenantCommandHandler(ITenantMemberRepository tenantMemberRepository, 
        IBillingIntegrationProxy billingIntegrationProxy, 
        ITenantRepository tenantRepository, 
        IUnitOfWork unitOfWork, 
        IPermissionRepository permissionRepository, 
        IRoleRepository roleRepository,
        ITenantCreationPolicy tenantCreationPolicy)
    {
        _tenantMemberRepository = tenantMemberRepository;
        _billingIntegrationProxy = billingIntegrationProxy;
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _permissionRepository = permissionRepository;
        _roleRepository = roleRepository;
        _tenantCreationPolicy = tenantCreationPolicy;
    }

    public async Task<Unit> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        await _tenantCreationPolicy.EnsureUserCanCreateTenantAsync(request.UserId);

        if (!await _tenantRepository.IsSubdomainAvailableAsync($"{request.SubDomain.ToLower().Replace(" ", "-")}.plataforma.com"))
            throw new Exception("subdomain already exists, pls chose another");

        var plan = await _billingIntegrationProxy.ValidatePlanAndPriceAsync(request.PlanId, request.PriceId);
        if(!plan.IsValid)
            throw new Exception(plan.ErrorMessage);


        var tenant = new Tenant(request.CompanyName, request.PlanId, plan.InternalPriceId!.Value, request.SubDomain);
        await _tenantRepository.AddAsync(tenant);

        var tenantMember = new TenantMember(tenant.Id, request.UserId, true);
        await _tenantMemberRepository.AddAsync(tenantMember);


        var allPermissions = await _permissionRepository.GetAllAsync();
        if (!allPermissions.Any())
            throw new Exception("isto nao deveria acontecer");

        var ownerRole = Role.CreateSystemOwner(tenant.Id, allPermissions);
        var adminRole = Role.CreateSystemAdmin(tenant.Id, allPermissions);

        await _roleRepository.AddAsync(ownerRole);
        await _roleRepository.AddAsync(adminRole);

        tenantMember.AddRole(ownerRole.Id);


        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}