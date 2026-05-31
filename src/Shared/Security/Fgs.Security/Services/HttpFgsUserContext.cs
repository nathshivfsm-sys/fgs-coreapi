using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Fgs.Security.Abstractions;
using Fgs.Security.Constants;
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

    public string? EntraObjectId =>
        User.FindFirst(JwtClaimTypes.EntraObjectId)?.Value
        ?? User.FindFirst("oid")?.Value
        ?? User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

    public long? TenantId =>
        long.TryParse(User.FindFirst(JwtClaimTypes.TenantId)?.Value, out var tenantId)
            ? tenantId
            : null;

    public long? CompanyId =>
        long.TryParse(User.FindFirst(JwtClaimTypes.CompanyId)?.Value, out var companyId)
            ? companyId
            : null;

    public IReadOnlyList<string> Roles =>
        User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

    public bool IsInRole(string roleCode) =>
        Roles.Contains(roleCode, StringComparer.OrdinalIgnoreCase);
}
