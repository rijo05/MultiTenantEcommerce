using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MultiTenantEcommerce.Application.Common.Interfaces.Services;
using JwtClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace MultiTenantEcommerce.Application.Identity.Auth.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    //public string GenerateToken(IEnumerable<Claim> claims)
    //{
    //    var claims = new List<Claim>
    //    {
    //        new(JwtClaimNames.Name, user.Name),
    //        new(JwtClaimNames.Email, user.Email.Value),
    //        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //        new("tenantId", user.TenantId.ToString()),
    //        new(JwtClaimNames.Jti, Guid.NewGuid().ToString())
    //    };

    //    if (roles != null)
    //        foreach (var role in roles.Distinct())
    //            claims.Add(new Claim("Role", role));

    //    if (permissions != null)
    //        foreach (var permission in permissions.Distinct())
    //            claims.Add(new Claim("Permission", permission));

    //    var isTenantMember = user is TenantMember;
    //    claims.Add(new Claim("isTenantMember", isTenantMember.ToString().ToLower(), ClaimValueTypes.Boolean));

    //    return GenerateJwtString(claims, DateTime.UtcNow.AddDays(7));
    //}

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

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}