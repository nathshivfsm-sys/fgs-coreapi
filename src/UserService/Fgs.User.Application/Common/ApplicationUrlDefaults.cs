namespace Fgs.User.Application.Common;

/// <summary>
/// Fallback URLs when configuration values are not set (local development).
/// </summary>
public static class ApplicationUrlDefaults
{
    public const string EntraCallbackRedirect = "https://localhost:8443/api/auth/entra/callback";

    public const string Dashboard = "https://localhost:8443/api/dashboard";

    public const string InviteStart = "https://localhost:8443/api/invite/start";
}
