using Scriban;

namespace MultiTenantEcommerce.Infrastructure.EmailService;
public class TemplateRenderer
{
    public string Render(string templateText, Dictionary<string, string> metadata)
    {
        var template = Template.Parse(templateText);
        return template.Render(metadata, memberRenamer: member => member.Name);
    }
}
