using MultiTenantEcommerce.Domain.Platform.Identity.Entities;
using MultiTenantEcommerce.Shared.Application.Interfaces;
using MultiTenantEcommerce.Shared.Domain.ValueObjects;

namespace MultiTenantEcommerce.Domain.Platform.Identity.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(Email email);
    Task<bool> ExistsByEmailAsync(Email email);
}