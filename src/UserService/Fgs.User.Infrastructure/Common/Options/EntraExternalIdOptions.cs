using Fgs.User.Application.Common;

namespace Fgs.User.Infrastructure.Common.Options;

public sealed class EntraExternalIdOptions
{
    public const string SectionName = "EntraExternalId";

    public string TenantId { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string Authority { get; set; } = "https://login.microsoftonline.com";

    /// <summary>
    /// Legacy credential key; prefer <c>Application:UiAuthCallbackUrl</c> / <see cref="LoginRedirectUri"/>.
    /// Must match the SPA redirect URI registered in Entra when used.
    /// </summary>
    public string RedirectUri { get; set; } = ApplicationUrlDefaults.UiAuthCallback;

    /// <summary>
    /// SPA-hosted OAuth callback URI for login and invite/signup (PKCE).
    /// </summary>
    public string LoginRedirectUri { get; set; } = ApplicationUrlDefaults.UiAuthCallback;

    public string Scopes { get; set; } = "openid profile email offline_access";

    public string TokenEndpoint { get; set; } = string.Empty;

    public string AuthorizeEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// Sign-up/sign-in user flow name (email OTP / code). Passed as the <c>p</c> query parameter.
    /// </summary>
    public string UserFlow { get; set; } = string.Empty;

    /// <summary>
    /// Password-based sign-up/sign-in user flow name. Used when the user prefers password auth.
    /// </summary>
    public string PasswordUserFlow { get; set; } = string.Empty;
}
