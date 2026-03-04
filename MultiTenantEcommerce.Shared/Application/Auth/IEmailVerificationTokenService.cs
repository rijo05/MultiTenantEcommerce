using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTenantEcommerce.Shared.Application.Auth;
public interface IEmailVerificationTokenService
{
    string GenerateToken(Guid customerId, TimeSpan expiresIn);
    bool TryValidateToken(string token, out Guid customerId);
}
