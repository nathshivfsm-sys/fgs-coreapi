namespace Fgs.Setup.Application.Abstractions.Credentials;

/// <summary>
/// Values for <c>AwsCredentials:DefaultVaultProvider</c>.
/// </summary>
public static class SecretVaultProviders
{
    /// <summary>
    /// Default. Credentials remain encrypted in the database via KMS envelope encryption; <see cref="ISecretVault"/> is unused.
    /// </summary>
    public const string Database = "Database";

    /// <summary>
    /// Feature-flagged AWS Secrets Manager vault. Wire DI and set
    /// <c>AwsCredentials:DefaultVaultProvider</c> to this value to activate; leave Compose on Database unless intentionally enabling.
    /// </summary>
    public const string AwsSecretsManager = "AwsSecretsManager";

    public static bool IsAwsSecretsManager(string? provider) =>
        string.Equals(provider, AwsSecretsManager, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// True for the default DB+KMS path (including blank and legacy <c>AWS</c> values that referred to KMS, not Secrets Manager).
    /// </summary>
    public static bool IsDatabase(string? provider) =>
        !IsAwsSecretsManager(provider);
}
