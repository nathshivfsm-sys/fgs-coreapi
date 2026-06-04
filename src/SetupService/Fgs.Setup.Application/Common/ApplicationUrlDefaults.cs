namespace Fgs.Setup.Application.Common;

/// <summary>
/// Fallback URLs when configuration values are not set (local development).
/// </summary>
public static class ApplicationUrlDefaults
{
    public const string EntraCallbackRedirect = "https://localhost:8443/api/v1/auth/entra/callback";

    public const string Dashboard = "https://localhost:8443/api/v1/dashboard";

    public const string InviteStart = "https://localhost:8443/api/v1/invite/start";
}
