using MultiTenantEcommerce.Shared.Domain.Utilities.Constants;

namespace MultiTenantEcommerce.Shared.Integration.DTOs;

public record EmailDataPacket(
    string ToEmail,
    string FromName,
    string? ReplyToEmail,
    EmailTemplateNames TemplateName,
    object DynamicData);