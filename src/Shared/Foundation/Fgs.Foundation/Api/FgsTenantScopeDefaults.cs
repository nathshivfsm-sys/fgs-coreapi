namespace Fgs.Foundation.Api;

/// <summary>
/// Default paths that do not require tenant/company scope headers.
/// Used by Swagger operation filters and aligned with <c>TenantScope:SkipPathPrefixes</c> at runtime.
/// </summary>
public static class FgsTenantScopeDefaults
{
    public const string ConfigurationSection = "TenantScope";

    public const string SkipPathPrefixesKey = "SkipPathPrefixes";

    public static readonly string[] SkipPathPrefixes =
    [
        "/api/v1/auth",
        "/api/v1/signup",
        "/api/v1/bff/signup",
        "/api/v1/invite",
        "/api/v1/invitations",
        "/api/v1/internal",
        "/api/v1/credential/resolved",
        "/api/v1/tenantprovisioning",
        "/api/v1/notification",
        "/health",
        "/swagger"
    ];
}
