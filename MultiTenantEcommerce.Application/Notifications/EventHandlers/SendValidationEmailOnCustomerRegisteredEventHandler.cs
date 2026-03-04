using MultiTenantEcommerce.Domain.Notifications.Interfaces;
using MultiTenantEcommerce.Shared.Infrastructure.Services;
using MultiTenantEcommerce.Shared.Integration.DTOs;
using MultiTenantEcommerce.Shared.Integration.Events.Commerce;
using MultiTenantEcommerce.Shared.Integration.Proxies;
using MultiTenantEcommerce.Shared.Utilities.Constants;

namespace MultiTenantEcommerce.Application.Notifications.EventHandlers;
public class SendValidationEmailOnCustomerRegisteredEventHandler : EmailHandlerBase<CustomerVerificationRequestedIntegrationEvent>
{
    private readonly ITenantIntegrationProxy _tenantIntegrationProxy;
    public SendValidationEmailOnCustomerRegisteredEventHandler(
        IEmailSender sender,
        ITemplateRender renderer,
        ITenantNotificationProfileRepository profileRepository,
        ITenantIntegrationProxy tenantIntegrationProxy)
        : base(sender, renderer, profileRepository) 
    {
        _tenantIntegrationProxy = tenantIntegrationProxy;
    }

    protected override async Task<EmailDataPacket> GetEmailDataAsync(
        CustomerVerificationRequestedIntegrationEvent evt)
    {
        var tenant = await _tenantIntegrationProxy.GetTenantProfileAsync(evt.TenantId);

        var validationUrl = $"https://{tenant.SubDomain}/validate?token={evt.Token}";

        object data = new
        {
            StoreName = tenant.StoreName,
            CustomerName = evt.CustomerName,
            CustomerEmail = evt.CustomerEmail,
            ValidationUrl = validationUrl
        };

        return new EmailDataPacket(evt.CustomerEmail, tenant.StoreName, tenant.Email,
            EmailTemplateNames.CustomerRegistered, data);
    }
}
