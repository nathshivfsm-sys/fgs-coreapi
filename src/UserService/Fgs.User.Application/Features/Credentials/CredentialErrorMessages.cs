namespace Fgs.User.Application.Features.Credentials;

public static class CredentialErrorMessages
{
    public const string TenantIdRequired = "TenantId is required.";
    public const string CompanyIdRequired = "CompanyId is required.";
    public const string ProviderCodeRequired = "Provider code is required.";
    public const string SecretNameRequired = "Secret name is required.";
    public const string SecretPayloadRequired = "Secret payload is required.";
    public const string ProviderTypeNotFound = "Credential provider type was not found.";
    public const string SecretNotFound = "Credential secret was not found.";
    public const string ProviderNotFound = "Credential provider was not found.";
    public const string TenantNotFound = "Tenant was not found.";
    public const string SecretAlreadyRevoked = "Credential secret is already revoked.";
    public const string KmsKeyArnNotConfigured = "AwsCredentials:KmsKeyArn is not configured.";
    public const string SecretAlreadyExists = "An active credential secret with this name already exists for the provider.";
    public const string SecretPayloadRequiredForUpdate = "Secret payload is required to update a credential secret.";
    public const string VaultAccessDenied =
        "AWS IAM principal is not authorized for Secrets Manager. Attach secretsmanager and KMS permissions (see deployment/aws/iam-fgs-credentials-secrets-policy.json).";
}
