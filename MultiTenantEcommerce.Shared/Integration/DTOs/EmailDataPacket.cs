using MultiTenantEcommerce.Shared.Utilities.Constants;

namespace MultiTenantEcommerce.Shared.Integration.DTOs;

public record EmailDataPacket(
    string ToEmail,
    string FromName,
    string? ReplyToEmail,
    EmailTemplateNames TemplateName,
    object DynamicData);