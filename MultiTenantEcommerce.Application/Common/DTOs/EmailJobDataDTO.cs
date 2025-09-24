using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Common.DTOs;
public record EmailJobDataDTO(
    Guid Id,
    Guid TenantId,
    string ToEmail,
    EmailTemplateNames TemplateName,
    Dictionary<string, string> Metadata,
    string? ReplyToEmail = null
);