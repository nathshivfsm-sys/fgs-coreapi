namespace Fgs.User.Infrastructure.Options;

public sealed class EntraExternalIdOptions
{
    public const string SectionName = "EntraExternalId";

    public string TenantId { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string Authority { get; set; } = "https://login.microsoftonline.com";

    /// <summary>
    /// Must exactly match the redirect URI registered in Entra and used in both authorize and token requests.
    /// </summary>
    public string RedirectUri { get; set; } = "http://localhost:5001/auth/callback";

    public string Scopes { get; set; } = "openid profile email";

    public string TokenEndpoint { get; set; } = string.Empty;

    public string AuthorizeEndpoint { get; set; } = string.Empty;

    /// <summary>
    /// Sign-up/sign-in user flow name. Passed as the <c>p</c> query parameter on the authorize request.
    /// </summary>
    public string UserFlow { get; set; } = string.Empty;
}
