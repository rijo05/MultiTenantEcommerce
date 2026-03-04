namespace MultiTenantEcommerce.Shared.Integration.DTOs;

public record PresignedUpload(
    string Key,
    string Url,
    string ContentType);