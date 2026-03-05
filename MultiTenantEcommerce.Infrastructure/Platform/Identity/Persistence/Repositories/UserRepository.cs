using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Platform.Identity.Entities;
using MultiTenantEcommerce.Domain.Platform.Identity.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Infrastructure.Persistence.Repositories;
using MultiTenantEcommerce.Infrastructure.Platform.Identity.Persistence.Context;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Infrastructure.Platform.Identity.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(IdentityDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<bool> ExistsByEmailAsync(Email email)
    {
        return await _dbContext.Set<User>()
            .AnyAsync(u => u.Email.Value == email.Value);
    }

    public async Task<User?> GetByEmailAsync(Email email)
    {
        return await _dbContext.Set<User>()
            .FirstOrDefaultAsync(u => u.Email.Value == email.Value);
    }
}