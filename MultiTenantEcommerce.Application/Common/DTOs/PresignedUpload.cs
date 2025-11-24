namespace MultiTenantEcommerce.Application.Common.DTOs;
public record PresignedUpload(
    string Key,
    string Url,
    string ContentType);
