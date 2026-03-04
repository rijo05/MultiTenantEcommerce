using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using MultiTenantEcommerce.Shared.Application.Auth;

namespace MultiTenantEcommerce.Infrastructure.Auth;
public class EmailVerificationTokenService : IEmailVerificationTokenService
{
    private readonly ITimeLimitedDataProtector _protector;

    public EmailVerificationTokenService(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("EmailVerification").ToTimeLimitedDataProtector();
    }

    public string GenerateToken(Guid customerId, TimeSpan expiresIn)
    {
        byte[] idBytes = customerId.ToByteArray();
        byte[] protectedBytes = _protector.Protect(idBytes, expiresIn);

        return Base64UrlEncoder.Encode(protectedBytes);
    }

    public bool TryValidateToken(string token, out Guid customerId)
    {
        customerId = Guid.Empty;
        try
        {
            byte[] protectedBytes = Base64UrlEncoder.DecodeBytes(token);

            byte[] idBytes = _protector.Unprotect(protectedBytes);

            customerId = new Guid(idBytes);
            return true;
        }
        catch (CryptographicException)
        {
            return false;
        }
    }
}
