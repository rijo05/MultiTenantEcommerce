namespace MultiTenantEcommerce.Shared.Infrastructure.Services;

public interface ITemplateRender
{
    string Render(string templateText, object model);
}