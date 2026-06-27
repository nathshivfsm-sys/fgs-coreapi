using System.Security.Claims;
using Fgs.Security.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Fgs.Security.Services;

/// <summary>
/// Entra External ID access tokens requested with <c>openid profile email</c> scopes often use the
/// Microsoft Graph audience and (for v1 tokens) the <c>sts.windows.net</c> issuer.
/// </summary>
public static class FgsEntraTokenValidation
{
    public const string MicrosoftGraphAudience = "00000003-0000-0000-c000-000000000000";

    public static IReadOnlyList<string> BuildValidIssuers(EntraExternalIdAuthOptions options)
    {
        var tenantId = options.TenantId.Trim('/');
        return
        [
            options.ResolveAuthority(),
            $"https://{tenantId}.ciamlogin.com/{tenantId}/v2.0",
            $"https://login.microsoftonline.com/{tenantId}/v2.0",
            $"https://sts.windows.net/{tenantId}/",
        ];
    }

    public static IReadOnlyList<string> BuildValidAudiences(string clientId) =>
    [
        clientId,
        $"api://{clientId}",
        MicrosoftGraphAudience
    ];

    public static TokenValidationParameters CreateValidationParameters(EntraExternalIdAuthOptions options)
    {
        var clientId = options.ClientId;
        var validIssuers = BuildValidIssuers(options);
        var validAudiences = BuildValidAudiences(clientId);

        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuers = validIssuers,
            IssuerValidator = (issuer, _, _) =>
                validIssuers.Any(valid => string.Equals(valid, issuer, StringComparison.OrdinalIgnoreCase))
                    ? issuer
                    : throw new SecurityTokenInvalidIssuerException(
                        $"Issuer '{issuer}' is not a trusted Entra issuer."),
            ValidateAudience = true,
            ValidAudiences = validAudiences,
            AudienceValidator = (audiences, securityToken, _) =>
            {
                var matched = audiences.FirstOrDefault(aud =>
                    validAudiences.Any(valid => string.Equals(valid, aud, StringComparison.OrdinalIgnoreCase)));

                if (matched is null)
                {
                    throw new SecurityTokenInvalidAudienceException(
                        $"Audience '{string.Join(", ", audiences)}' is not valid.");
                }

                if (string.Equals(matched, MicrosoftGraphAudience, StringComparison.OrdinalIgnoreCase))
                {
                    var appId = ReadClaimValue(securityToken, "appid");
                    if (!string.Equals(appId, clientId, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new SecurityTokenInvalidAudienceException(
                            "Microsoft Graph audience token appid does not match configured Entra client id.");
                    }
                }

                return true;
            },
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            TryAllIssuerSigningKeys = true,
            NameClaimType = "name",
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.FromMinutes(5)
        };
    }

    public static bool ValidateGraphAudienceAppId(ClaimsPrincipal? principal, string clientId)
    {
        var aud = principal?.FindFirst("aud")?.Value;
        if (!string.Equals(aud, MicrosoftGraphAudience, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var appId = principal?.FindFirst("appid")?.Value;
        return string.Equals(appId, clientId, StringComparison.OrdinalIgnoreCase);
    }

    private static string? ReadClaimValue(SecurityToken securityToken, string claimType)
    {
        if (securityToken is System.IdentityModel.Tokens.Jwt.JwtSecurityToken jwt
            && jwt.Payload.TryGetValue(claimType, out var jwtValue))
        {
            return jwtValue?.ToString();
        }

        if (securityToken is JsonWebToken jsonWebToken)
        {
            return jsonWebToken.GetPayloadValue<string?>(claimType);
        }

        return null;
    }
}
