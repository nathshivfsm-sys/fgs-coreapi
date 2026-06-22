using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Fgs.Security.Abstractions;
using Fgs.Security.Constants;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Http;

namespace Fgs.Security.Services;

public sealed class HttpFgsUserContext(IHttpContextAccessor httpContextAccessor) : IFgsUserContext
{
    private ClaimsPrincipal User => httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();

    public bool IsAuthenticated => User.Identity?.IsAuthenticated == true;

    public Guid? UserId =>
        Guid.TryParse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out var userId)
            ? userId
            : null;

    public string? Email => User.FindFirst(JwtRegisteredClaimNames.Email)?.Value
        ?? User.FindFirst("preferred_username")?.Value;

    public string? DisplayName =>
        User.FindFirst("name")?.Value
        ?? User.FindFirst(ClaimTypes.Name)?.Value
        ?? CombineNames(
            User.FindFirst("given_name")?.Value,
            User.FindFirst(JwtRegisteredClaimNames.GivenName)?.Value,
            User.FindFirst("family_name")?.Value,
            User.FindFirst(JwtRegisteredClaimNames.FamilyName)?.Value);

    public string? EntraObjectId =>
        User.FindFirst(JwtClaimTypes.EntraObjectId)?.Value
        ?? User.FindFirst("oid")?.Value
        ?? User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

    public long? TenantId => FgsRequestAuthContext.ExtractTenantScope(httpContextAccessor.HttpContext).TenantId;

    public long? CompanyId => FgsRequestAuthContext.ExtractTenantScope(httpContextAccessor.HttpContext).CompanyId;

    public IReadOnlyList<string> Roles =>
        User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

    public bool IsInRole(string roleCode) =>
        Roles.Contains(roleCode, StringComparer.OrdinalIgnoreCase);

    private static string? CombineNames(params string?[] parts)
    {
        var values = parts
            .Where(part => !string.IsNullOrWhiteSpace(part))
            .Select(part => part!.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        return values.Length == 0 ? null : string.Join(' ', values);
    }
}
