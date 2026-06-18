namespace Fgs.Security.Authorization;

public static class FgsAuthorizationPolicies
{
    public const string RequireTenantAdmin = nameof(RequireTenantAdmin);

    public const string RequirePlatformAdmin = nameof(RequirePlatformAdmin);

    public const string RequireAuthenticatedJwt = nameof(RequireAuthenticatedJwt);
}
