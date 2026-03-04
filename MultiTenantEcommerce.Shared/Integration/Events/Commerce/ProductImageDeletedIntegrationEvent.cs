namespace MultiTenantEcommerce.Shared.Integration.Events.Commerce;

public record ProductImageDeletedIntegrationEvent(
    Guid TenantId,
    Guid ProductId,
    Guid ImageId,
    string ImageKey) : IntegrationEvent(TenantId);
