namespace Fgs.User.Infrastructure.Options;

public sealed class EntraExternalIdOptions
{
    public const string SectionName = "EntraExternalId";

    public string TenantId { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string Authority { get; set; } = "https://login.microsoftonline.com";

    public string RedirectUri { get; set; } = "https://localhost:5001/api/auth/entra/callback";

    public string Scopes { get; set; } = "openid profile email";

    public string TokenEndpoint { get; set; } = string.Empty;

    public string AuthorizeEndpoint { get; set; } = string.Empty;
}
