using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Platform.Billing.Entities;
using MultiTenantEcommerce.Domain.Platform.Billing.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Infrastructure.Platform.Billing.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Platform.Billing.Persistence.Repositories;

public class SubscriptionPlanRepository : Repository<SubscriptionPlan>, ISubscriptionPlanRepository
{
    public SubscriptionPlanRepository(BillingDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<SubscriptionPlan?> GetByStripeProductId(string productId)
    {
        return await _dbContext.Set<SubscriptionPlan>()
            .Include(x => x.PlanLimits)
            .Include(x => x.Prices)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.StripeProductId == productId);
    }


    public override async Task<List<SubscriptionPlan>> GetAllAsync()
    {
        return await _dbContext.Set<SubscriptionPlan>()
            .Include(x => x.PlanLimits)
            .Include(x => x.PlanLimits)
            .ToListAsync();
    }

    public override async Task<SubscriptionPlan?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<SubscriptionPlan>()
            .Include(x => x.Prices)
            .Include(x => x.PlanLimits)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}