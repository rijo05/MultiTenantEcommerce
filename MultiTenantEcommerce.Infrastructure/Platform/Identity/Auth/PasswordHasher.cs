using Microsoft.AspNetCore.Identity;
using MultiTenantEcommerce.Shared.Application.Auth;

namespace MultiTenantEcommerce.Infrastructure.Platform.Identity.Auth;
internal sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool Verify(string password, string passwordHash)
    {
        var result = _hasher.VerifyHashedPassword(null!, passwordHash, password);

        return result != PasswordVerificationResult.Failed;
    }
}
