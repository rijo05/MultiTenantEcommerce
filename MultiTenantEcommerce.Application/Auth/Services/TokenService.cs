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

    public string CreateSessionToken(UserBase user)
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

        return GenerateToken(claims, DateTime.UtcNow.AddDays(1));
    }

    public string CreateImageToken(Guid ProductId, Guid TenantId)
    {
        var claims = new List<Claim>
        {
            new Claim("ProductId", ProductId.ToString()),
            new Claim("TenantId", TenantId.ToString())
        };

        return GenerateToken(claims, DateTime.UtcNow.AddHours(100));
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _config["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = _config["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };

        return tokenHandler.ValidateToken(token, parameters, out _);
    }

    private string GenerateToken(List<Claim> claims, DateTime time)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: time,
                signingCredentials: credentials
                );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
