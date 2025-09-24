using MultiTenantEcommerce.Domain.Common.Entities;

namespace MultiTenantEcommerce.Domain.Templates.Entities;
public class EmailTemplate : BaseEntity
{
    public string TemplateName { get; private set; }
    public bool IsActive { get; private set; }
    public string Subject { get; private set; }
    public string HtmlContent { get; private set; }
    public string? TextContent { get; private set; }

    public EmailTemplate(string templateName, bool isActive, string subject, string htmlContent)
    {
        TemplateName = templateName;
        IsActive = isActive;
        Subject = subject;
        HtmlContent = htmlContent;
    }

    public void Deactivate() => IsActive = false;
}
