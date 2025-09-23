namespace MultiTenantEcommerce.Domain.Common.Events;
public interface IEmailEvent
{
    string TemplateName { get; }
}
