using MultiTenantEcommerce.Domain.Platform.Identity.Entities;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;
using MultiTenantEcommerce.Shared.Infrastructure.Persistence;

namespace MultiTenantEcommerce.Domain.Platform.Identity.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(Email email);
    Task<bool> ExistsByEmailAsync(Email email);
}