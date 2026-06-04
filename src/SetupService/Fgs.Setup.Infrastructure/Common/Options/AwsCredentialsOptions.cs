namespace Fgs.Setup.Infrastructure.Common.Options;

public sealed class AwsCredentialsOptions
{
    public const string SectionName = "AwsCredentials";

    public string Region { get; set; } = "us-east-1";

    /// <summary>Optional. When set with <see cref="SecretAccessKey"/>, used for local development. Leave empty in production (IAM role).</summary>
    public string? AccessKeyId { get; set; }

    /// <summary>Optional. When set with <see cref="AccessKeyId"/>, used for local development. Leave empty in production (IAM role).</summary>
    public string? SecretAccessKey { get; set; }

    public string KmsKeyArn { get; set; } = string.Empty;

    /// <summary>Prefix for tenant S3 bucket names, e.g. fgs-dev-tenant.</summary>
    public string BucketNamePrefix { get; set; } = "fgs-prod-tenant";

    /// <summary>Application segment in secret paths (default: fsm). Full pattern: {Environment}/{ApplicationSlug}/{tenantCode}/{providerCode}.</summary>
    public string ApplicationSlug { get; set; } = "fsm";

    /// <summary>Legacy alias for <see cref="ApplicationSlug"/>.</summary>
    public string SecretNamePrefix
    {
        get => ApplicationSlug;
        set => ApplicationSlug = value;
    }

    public string DefaultVaultProvider { get; set; } = "AWS";

    public int CacheTtlSeconds { get; set; } = 300;

    public bool EnableLocalProfileFallback { get; set; } = true;

    /// <summary>
    /// When true together with Development environment, exposes GET /api/v1/credentials/test/{secretId}/resolve.
    /// Must remain false in production.
    /// </summary>
    public bool EnableTestSecretEndpoint { get; set; }
}
