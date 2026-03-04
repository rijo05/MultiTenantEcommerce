namespace MultiTenantEcommerce.Domain.Platform.Billing.Entities;

public record PlanLimits(
    int MaxProducts,
    int MaxCategories,
    int MaxImagesPerProduct,
    int MaxTenantMembers,
    long MaxStorageBytes,
    decimal TransactionFee,
    bool AcessToWebhooks);