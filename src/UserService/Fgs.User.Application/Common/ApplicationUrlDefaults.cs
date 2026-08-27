namespace Fgs.User.Application.Common;

/// <summary>
/// Fallback URLs when configuration values are not set.
/// Prefer <see cref="ApplicationPublicUrlResolver"/> with <c>Application:PublicBaseUrl</c> per environment.
/// </summary>
public static class ApplicationUrlDefaults
{
    public const string EntraCallbackPath = "/api/v1/auth/entra/callback";

    public const string DashboardPath = "/api/v1/dashboard";

    public const string InviteStartPath = "/api/v1/invite/start";

    public const string EntraCallbackRedirect = "https://developer.fsm.com" + EntraCallbackPath;

    public const string Dashboard = "https://developer.fsm.com" + DashboardPath;

    public const string InviteStart = "https://developer.fsm.com" + InviteStartPath;
}
