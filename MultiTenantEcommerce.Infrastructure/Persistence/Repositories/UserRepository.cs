using Microsoft.EntityFrameworkCore;
using MultiTenantEcommerce.Domain.Platform.Identity.Entities;
using MultiTenantEcommerce.Domain.Platform.Identity.Interfaces;
using MultiTenantEcommerce.Infrastructure.Persistence.Context;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Infrastructure.Persistence.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext appDbContext) : base(appDbContext)
    {
    }

    public async Task<bool> ExistsByEmailAsync(Email email)
    {
        return await _appDbContext.Users
            .AnyAsync(u => u.Email.Value == email.Value);
    }

    public async Task<User?> GetByEmailAsync(Email email)
    {
        return await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.Email.Value == email.Value);
    }
}