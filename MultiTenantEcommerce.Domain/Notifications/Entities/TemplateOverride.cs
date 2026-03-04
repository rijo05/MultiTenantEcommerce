using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Utilities.Constants;

namespace MultiTenantEcommerce.Domain.Notifications.Entities;

public class TemplateOverride : BaseEntity
{
    private TemplateOverride()
    {
    }

    internal TemplateOverride(EmailTemplateNames templateName, string? subject, string? greeting, string? mainText)
    {
        TemplateName = templateName;
        CustomSubject = subject;
        CustomGreeting = greeting;
        CustomMainText = mainText;
    }

    public EmailTemplateNames TemplateName { get; private set; }
    public string? CustomSubject { get; private set; }
    public string? CustomGreeting { get; private set; }
    public string? CustomMainText { get; private set; }

    internal void Update(string? subject, string? greeting, string? mainText)
    {
        CustomSubject = subject;
        CustomGreeting = greeting;
        CustomMainText = mainText;
    }
}