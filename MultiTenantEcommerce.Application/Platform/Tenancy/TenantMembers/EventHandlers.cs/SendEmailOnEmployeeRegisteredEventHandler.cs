using MultiTenantEcommerce.Domain.Platform.Tenancy.Events;
using MultiTenantEcommerce.Domain.Platform.Tenancy.Interfaces.Repositories;

namespace MultiTenantEcommerce.Application.Platform.Tenancy.TenantMembers.EventHandlers.cs;

public class SendEmailOnTenantMemberRegisteredEventHandler : IEventHandler<TenantMemberRegisteredEvent>
{
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly ITenantMemberRepository _employeeRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ITenantRepository _tenantRepository;

    public SendEmailOnTenantMemberRegisteredEventHandler(IEmailQueueRepository emailQueueRepository,
        IRoleRepository roleRepository,
        ITenantMemberRepository employeeRepository,
        ITenantRepository tenantRepository)
    {
        _emailQueueRepository = emailQueueRepository;
        _roleRepository = roleRepository;
        _employeeRepository = employeeRepository;
        _tenantRepository = tenantRepository;
    }

    public async Task HandleAsync(TenantMemberRegisteredEvent domainEvent)
    {
        var employee = await _employeeRepository.GetByIdAsync(domainEvent.TenantMemberId)
                       ?? throw new Exception("TenantMember not found");

        var roles = await _roleRepository.GetByIdsAsync(employee.TenantMemberRoles.Select(x => x.RoleId).ToList());

        var tenant = await _tenantRepository.GetByIdAsync(domainEvent.TenantId)
                     ?? throw new Exception("Tenant not found");

        var metadata = new Dictionary<string, string>
        {
            [EmailMetadataKeys.TenantMemberName] = employee.Name,
            [EmailMetadataKeys.ChangePasswordLink] = "www.link.com", //############### TODO
            [EmailMetadataKeys.RolesHtml] = $"<ul>{string.Join("", roles.Select(er => $"<li>{er.Name}</li>"))}</ul>",
            [EmailMetadataKeys.TenantName] = tenant.Name
        };

        var email = new EmailJobDataDTO(
            Guid.Empty,
            domainEvent.TenantId,
            tenant.Name,
            employee.Email.Value,
            EmailTemplateNames.TenantMemberRegistered,
            domainEvent.EventPriority,
            metadata,
            tenant.Email.Value
        );

        await _emailQueueRepository.EnqueueEmailAsync(email);
    }
}