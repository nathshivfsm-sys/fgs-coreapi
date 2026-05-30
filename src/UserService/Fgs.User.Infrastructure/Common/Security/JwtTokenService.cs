using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fgs.Security.Constants;
using Fgs.Security.Options;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fgs.User.Infrastructure.Common.Security;

public sealed class JwtTokenService(IOptions<JwtOptions> options) : IJwtTokenService
{
    private readonly JwtOptions _options = options.Value;

    public string CreateToken(FgsUser user, IReadOnlyList<string> roleCodes)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtClaimTypes.TenantId, user.TenantId.ToString()),
            new(JwtClaimTypes.CompanyId, user.CompanyId.ToString())
        };

        foreach (var roleCode in roleCodes)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleCode));
        }

        if (!string.IsNullOrEmpty(user.EntraObjectId))
        {
            claims.Add(new Claim(JwtClaimTypes.EntraObjectId, user.EntraObjectId));
        }

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
