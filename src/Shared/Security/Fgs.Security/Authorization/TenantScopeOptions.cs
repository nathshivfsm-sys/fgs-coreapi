namespace Fgs.Security.Authorization;

public sealed class TenantScopeOptions
{
    public const string SectionName = "TenantScope";

    public string[] SkipPathPrefixes { get; set; } =
    [
        "/api/v1/login",
        "/api/v1/auth",
        "/api/v1/signup",
        "/api/v1/invitations",
        "/api/v1/internal",
        "/health",
        "/swagger"
    ];
}
