using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Tenants.Entities;
using MultiTenantEcommerce.Domain.Tenants.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
public class SubscriptionPlanRepository : Repository<SubscriptionPlan>, ISubscriptionPlanRepository
{
    public SubscriptionPlanRepository(AppDbContext appDbContext) : base(appDbContext) { }


    public override async Task<List<SubscriptionPlan>> GetAllAsync()
    {
        return await _appDbContext.SubscriptionPlans
            .Include(x => x.PlanLimits)
            .ToListAsync();
    }

    public override async Task<SubscriptionPlan?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.SubscriptionPlans
            .Include(x => x.PlanLimits)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<SubscriptionPlan?> GetByStripeProductId(string productId)
    {
        return await _appDbContext.SubscriptionPlans
            .Include(x => x.PlanLimits)
            .Include(x => x.Prices)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.StripeProductId == productId);
    }
}
