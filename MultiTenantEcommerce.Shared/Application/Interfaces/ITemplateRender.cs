namespace MultiTenantEcommerce.Shared.Application.Interfaces;

public interface ITemplateRender
{
    string Render(string templateText, object model);
}