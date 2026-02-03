using MultiTenantEcommerce.Application.Common.Interfaces.CQRS;
using MultiTenantEcommerce.Application.Common.Interfaces.Persistence;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Application.Payment.OrderPayment.Interfaces;
using MultiTenantEcommerce.Domain.Enums;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Domain.Users.Entities;
using MultiTenantEcommerce.Domain.Users.Entities.Permissions;
using MultiTenantEcommerce.Domain.Users.Interfaces;
using MultiTenantEcommerce.Domain.Users.Interfaces.Permissions;
using MultiTenantEcommerce.Domain.ValueObjects;

namespace MultiTenantEcommerce.Application.Auth.Commands.CreateTenant;
public class CreateTenantCommandHandler : ICommandHandler<CreateTenantCommand, string>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ITokenService _tokenService;
    private readonly IRoleRepository _roleRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IPaymentProviderFactory _paymentProviderFactory;

    public CreateTenantCommandHandler(ITenantRepository tenantRepository,
        IUnitOfWork unitOfWork,
        IEmployeeRepository employeeRepository,
        ITokenService tokenService,
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        ISubscriptionPlanRepository subscriptionPlanRepository,
        IPaymentProviderFactory paymentProviderFactory)
    {
        _tenantRepository = tenantRepository;
        _unitOfWork = unitOfWork;
        _employeeRepository = employeeRepository;
        _tokenService = tokenService;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _paymentProviderFactory = paymentProviderFactory;
    }

    public async Task<string> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
    {
        var existingTenant = await _tenantRepository.GetByCompanyNameAllIncluded(request.CompanyName);

        if (existingTenant is not null)
            throw new Exception("Company with this name already exists.");

        var plan = await _subscriptionPlanRepository.GetByIdAsync(request.PlanId) ??
            throw new Exception("Plan doesnt exist");

        var tenant = new Tenant(request.CompanyName, new Email(request.CompanyEmail), plan);
        await _tenantRepository.AddAsync(tenant);

        var ownerRole = new Role(tenant.Id, SystemRoles.Owner, "Full access");
        var adminRole = new Role(tenant.Id, SystemRoles.Admin, "Full access except tenant settings");

        var allPermissions = await _permissionRepository.GetAllAsync();

        foreach (var perm in allPermissions)
        {
            ownerRole.AddPermission(perm.Id);

            if (!perm.Area.Equals("tenant", StringComparison.OrdinalIgnoreCase))
            {
                adminRole.AddPermission(perm.Id);
            }
        }

        ownerRole.MarkRoleAsSystemRole();
        adminRole.MarkRoleAsSystemRole();

        await _roleRepository.AddAsync(ownerRole);
        await _roleRepository.AddAsync(adminRole);


        var employee = new Employee(
            tenant.Id,
            request.OwnerName,
            new Email(request.OwnerEmail),
            new Password(request.Password), new List<Guid> { ownerRole.Id });

        await _employeeRepository.AddAsync(employee);

        await _unitOfWork.CommitAsync();

        var roleNames = new List<string> { ownerRole.Name };
        var permissionNames = allPermissions.Select(p => p.Name).ToList();


        var paymentProvider = _paymentProviderFactory.GetProvider(PaymentMethod.Stripe); //se calhar no futuro tiro isto pq apenas tenho a stripe e a stripe implementa todos os paymentMethods, nao preciso de estar a fazer para mbway, cartao, paypal...

        var customerId = await paymentProvider.CreateCustomerAsync(
                tenant.Name,
                tenant.Email.Value,
                tenant.Id);

        tenant.SetStripeCustomerId(customerId);

        var checkoutUrl = await paymentProvider.CreateCheckoutSessionAsync(
            tenant.StripeCustomerId,
            plan.StripePriceId,
            "https://www.youtube.com/",
            "https://x.com/home"
        );

        return checkoutUrl;
    }
}
