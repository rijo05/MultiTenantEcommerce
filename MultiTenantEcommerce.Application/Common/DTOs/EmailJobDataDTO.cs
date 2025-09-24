using MultiTenantEcommerce.Domain.Enums;

namespace MultiTenantEcommerce.Application.Common.DTOs;
public record EmailJobDataDTO(
    Guid Id,
    Guid TenantId,
    string FromName,
    string ToEmail,
    EmailTemplateNames TemplateName,
    EventPriority Priority,
    Dictionary<string, string> Metadata,
    string? ReplyToEmail = null
);