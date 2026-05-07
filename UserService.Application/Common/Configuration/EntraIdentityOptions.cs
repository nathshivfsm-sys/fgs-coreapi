namespace UserService.Application.Common.Configuration;

/// <summary>
/// OIDC issuer used when linking Microsoft Entra External ID users to internal profiles (matches the <c>iss</c> claim).
/// </summary>
public sealed class EntraIdentityOptions
{
    public const string SectionName = "Entra";

    /// <summary>
    /// Example: <c>https://login.microsoftonline.com/{tenantId}/v2.0</c> or your CIAM issuer URL.
    /// </summary>
    public string OidcIssuer { get; set; } = string.Empty;
}
