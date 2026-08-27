using Fgs.Contracts.Api;
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
    /// Must exactly match the redirect URI registered in Entra and used in both authorize and token requests
    /// for the signup/invite callback (API-hosted).
    /// </summary>
    public string RedirectUri { get; set; } = ApplicationUrlDefaults.EntraCallbackRedirect;

    /// <summary>
    /// SPA-hosted login callback URI used only for returning-user login (Option A).
    /// </summary>
    public string LoginRedirectUri { get; set; } = ApplicationUrlDefaults.EntraCallbackRedirect;

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

