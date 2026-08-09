namespace Fgs.User.Application.Common;

/// <summary>
/// Fallback URLs when configuration values are not set.
/// </summary>
public static class ApplicationUrlDefaults
{
    public const string EntraCallbackRedirect = "https://developer.fsm.com/api/v1/auth/entra/callback";

    public const string Dashboard = "https://developer.fsm.com/api/v1/dashboard";

    public const string InviteStart = "https://developer.fsm.com/api/v1/invite/start";
}
