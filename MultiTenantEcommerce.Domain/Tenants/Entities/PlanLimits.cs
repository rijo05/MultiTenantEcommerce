namespace MultiTenantEcommerce.Domain.Tenants.Entities;
public record PlanLimits(
    int MaxProducts,
    int MaxCategories,
    int MaxImagesPerProduct,
    int MaxEmployees,
    int MaxStorageBytes,
    decimal TransactionFee);
