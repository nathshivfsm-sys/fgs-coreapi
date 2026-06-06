namespace Fgs.Setup.Application.Features.Credentials;

internal static class CredentialErrorMessages
{
    public const string ProviderNotFound = "Credential provider type was not found.";
    public const string ProviderInactive = "Credential provider type is inactive.";
    public const string GlobalCredentialExists = "A global credential already exists for this provider type.";
    public const string TenantCredentialExists = "A tenant credential already exists for this provider type.";
    public const string GlobalCredentialNotFound = "Global credential was not found.";
    public const string TenantCredentialNotFound = "Tenant credential was not found.";
    public const string InvalidScope = "Credential scope is invalid.";
    public const string InvalidPayload = "Credential payload must be valid UTF-8 text or JSON.";
    public const string TenantContextRequired = "Tenant and company context are required for tenant credentials.";
    public const string SecretResolveDisabled = "Credential secret resolution endpoint is disabled.";
}
