using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using MultiTenantEcommerce.Domain.Users.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace MultiTenantEcommerce.Application.Auth.Services;
public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string CreateToken(UserBase user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtClaimNames.Name, user.Name),
            new Claim(JwtClaimNames.Email, user.Email.Value),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("tenantId", user.TenantId.ToString()),
            new Claim(JwtClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (user is Employee)
        {
            var employee = user as Employee;

            var roles = employee.EmployeeRoles.Select(x => x.Role.Name).Distinct();
            var permissions = employee.EmployeeRoles.SelectMany(x => x.Role.Permissions.Select(x => x.Name)).Distinct();

            foreach (var roleName in roles)
            {
                claims.Add(new Claim("Role", roleName));
            }

            foreach (var permissionName in permissions)
            {
                claims.Add(new Claim("Permission", permissionName));
            }

            claims.Add(new Claim("isEmployee", "true", ClaimValueTypes.Boolean));
        }
        else
            claims.Add(new Claim("isEmployee", "false", ClaimValueTypes.Boolean));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credentials
                );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
