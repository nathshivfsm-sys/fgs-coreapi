namespace Fgs.User.Application.Common;

/// <summary>
/// Fallback URLs when configuration values are not set.
/// Prefer <see cref="ApplicationPublicUrlResolver"/> with <c>Application:PublicBaseUrl</c> /
/// <c>Application:UiAuthCallbackUrl</c> per environment.
/// </summary>
public static class ApplicationUrlDefaults
{
    public const string InviteStartPath = "/api/v1/invite/start";

    /// <summary>
    /// SPA Entra OAuth redirect fallback when no UI callback is configured.
    /// </summary>
    public const string UiAuthCallback = "https://developer.fsm.com/auth/callback";

    public const string InviteStart = "https://developer.fsm.com" + InviteStartPath;
}
