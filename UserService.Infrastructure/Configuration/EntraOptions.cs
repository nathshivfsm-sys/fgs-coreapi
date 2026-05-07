namespace UserService.Infrastructure.Configuration;

public sealed class EntraOptions
{
    public const string SectionName = "Entra";

    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>Base URL for Microsoft Graph (defaults to global cloud).</summary>
    public string GraphBaseUrl { get; set; } = "https://graph.microsoft.com";

    /// <summary>Default verified domain for UPN, e.g. <c>contoso.onmicrosoft.com</c>.</summary>
    public string DefaultDomain { get; set; } = string.Empty;
}
