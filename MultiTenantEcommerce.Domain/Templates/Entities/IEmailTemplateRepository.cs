using MultiTenantEcommerce.Domain.Common.Interfaces;
using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Domain.Templates.Entities;
public interface IEmailTemplateRepository : IRepository<EmailTemplate>
{
    public Task<EmailTemplate?> GetDefaultTemplateByTemplateName(EmailTemplateNames templateName);
}
