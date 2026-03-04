using MultiTenantEcommerce.Domain.Notifications.ValueObjects;
using MultiTenantEcommerce.Shared.Domain.Abstractions;
using MultiTenantEcommerce.Shared.Utilities.Constants;

namespace MultiTenantEcommerce.Domain.Notifications.Entities;

public class TenantNotificationProfile : BaseEntity
{
    private readonly List<TemplateOverride> _overrides = new();

    private TenantNotificationProfile()
    {
    }

    private TenantNotificationProfile(Guid tenantId, ThemeSettings theme)
    {
        TenantId = tenantId;
        Theme = theme;
    }

    public Guid TenantId { get; private set; }
    public ThemeSettings Theme { get; private set; }
    public IReadOnlyCollection<TemplateOverride> Overrides => _overrides.AsReadOnly();

    public static TenantNotificationProfile CreateDefault(Guid tenantId)
    {
        return new TenantNotificationProfile(tenantId, ThemeSettings.Default());
    }

    public void UpdateTheme(ThemeSettings newTheme)
    {
        Theme = newTheme ?? throw new ArgumentNullException(nameof(newTheme));
    }

    public void SetTemplateOverride(EmailTemplateNames templateName, string? subject, string? greeting,
        string? mainText)
    {
        var existingOverride = _overrides.FirstOrDefault(o => o.TemplateName == templateName);

        if (existingOverride != null)
            existingOverride.Update(subject, greeting, mainText);
        else
            _overrides.Add(new TemplateOverride(templateName, subject, greeting, mainText));
    }
}