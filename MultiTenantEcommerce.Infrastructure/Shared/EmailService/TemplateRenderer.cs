using MultiTenantEcommerce.Shared.Application.Interfaces;
using Scriban;

namespace MultiTenantEcommerce.Infrastructure.Shared.EmailService;

public class TemplateRenderer : ITemplateRender
{
    public string Render(string templateText, object model)
    {
        var template = Template.Parse(templateText);

        if (template.HasErrors)
        {
            var errors = string.Join(", ", template.Messages.Select(x => x.Message));
            throw new InvalidOperationException($"Erro de sintaxe no template HTML estático: {errors}");
        }

        return template.Render(model, member => member.Name);
    }
}