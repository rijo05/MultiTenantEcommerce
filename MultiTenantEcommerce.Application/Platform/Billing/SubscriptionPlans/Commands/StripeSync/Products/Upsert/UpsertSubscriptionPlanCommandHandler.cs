using System.Globalization;
using MediatR;
using MultiTenantEcommerce.Domain.Billing.Entities;
using MultiTenantEcommerce.Domain.Platform.Billing.Entities;
using MultiTenantEcommerce.Domain.Platform.Billing.Interfaces;

namespace MultiTenantEcommerce.Application.Platform.Billing.SubscriptionPlans.Commands.StripeSync.Products.Upsert;

public class UpsertSubscriptionPlanCommandHandler : ICommandHandler<UpsertSubscriptionPlanCommand, Unit>
{
    private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpsertSubscriptionPlanCommandHandler(ISubscriptionPlanRepository subscriptionPlanRepository,
        IUnitOfWork unitOfWork)
    {
        _subscriptionPlanRepository = subscriptionPlanRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpsertSubscriptionPlanCommand request, CancellationToken cancellationToken)
    {
        var limits = ParseLimitsFromMetadata(request.Metadata);

        var plan = await _subscriptionPlanRepository.GetByStripeProductId(request.StripeProductId);

        if (plan == null)
        {
            plan = new SubscriptionPlan(request.Name, request.StripeProductId, limits);
            await _subscriptionPlanRepository.AddAsync(plan);
        }
        else
        {
            plan.Update(request.Name, limits);
        }

        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }

    private PlanLimits ParseLimitsFromMetadata(Dictionary<string, string> metadata)
    {
        var maxProducts = GetInt(metadata, "MaxProducts", 20);
        var maxCategories = GetInt(metadata, "MaxCategories", 5);
        var maxImages = GetInt(metadata, "MaxImagesPerProduct", 3);
        var maxTenantMembers = GetInt(metadata, "MaxTenantMembers", 2);

        var storageInGb = GetDecimal(metadata, "MaxStorageGB", 1.0m);
        var maxStorageBytes = ConvertGbToBytes(storageInGb);

        var fee = GetDecimal(metadata, "TransactionFee", 0.05m);
        var accessToWebhooks = GetBoolean(metadata, "AccessToWebhooks", false);

        return new PlanLimits(maxProducts, maxCategories, maxImages, maxTenantMembers, maxStorageBytes, fee,
            accessToWebhooks);
    }

    private long ConvertGbToBytes(decimal gb)
    {
        return (long)(gb * 1024m * 1024m * 1024m);
    }

    private int GetInt(Dictionary<string, string> metadata, string key, int defaultValue)
    {
        if (metadata != null &&
            metadata.TryGetValue(key, out var value) &&
            int.TryParse(value, out var result))
            return result;
        return defaultValue;
    }

    private decimal GetDecimal(Dictionary<string, string> metadata, string key, decimal defaultValue)
    {
        if (metadata != null &&
            metadata.TryGetValue(key, out var value) &&
            decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            return result;
        return defaultValue;
    }

    private bool GetBoolean(Dictionary<string, string> metadata, string key, bool defaultValue)
    {
        if (metadata != null &&
            metadata.TryGetValue(key, out var value) &&
            bool.TryParse(value, out var result))
            return result;
        return defaultValue;
    }
}