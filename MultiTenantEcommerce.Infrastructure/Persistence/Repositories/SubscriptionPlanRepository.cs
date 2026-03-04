using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Platform.Billing.Entities;
using MultiTenantEcommerce.Domain.Platform.Billing.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class SubscriptionPlanRepository : Repository<SubscriptionPlan>, ISubscriptionPlanRepository
{
    public SubscriptionPlanRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<SubscriptionPlan?> GetByStripeProductId(string productId)
    {
        return await _appDbContext.SubscriptionPlans
            .Include(x => x.PlanLimits)
            .Include(x => x.Prices)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.StripeProductId == productId);
    }


    public override async Task<List<SubscriptionPlan>> GetAllAsync()
    {
        return await _appDbContext.SubscriptionPlans
            .Include(x => x.PlanLimits)
            .Include(x => x.PlanLimits)
            .ToListAsync();
    }

    public override async Task<SubscriptionPlan?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.SubscriptionPlans
            .Include(x => x.Prices)
            .Include(x => x.PlanLimits)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}