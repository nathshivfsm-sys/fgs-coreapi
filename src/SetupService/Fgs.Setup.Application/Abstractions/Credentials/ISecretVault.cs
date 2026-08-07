namespace Fgs.Setup.Application.Abstractions.Credentials;

/// <summary>
/// Optional external secret vault. Used only when
/// <c>AwsCredentials:DefaultVaultProvider</c> is <see cref="SecretVaultProviders.AwsSecretsManager"/>.
/// The default <see cref="SecretVaultProviders.Database"/> path keeps credentials in DB + KMS and does not call this vault.
/// </summary>
public interface ISecretVault
{
    string ProviderName { get; }

    Task<string?> GetSecretStringAsync(string secretId, CancellationToken cancellationToken = default);

    Task CreateSecretAsync(
        string secretId,
        string secretString,
        string? description = null,
        CancellationToken cancellationToken = default);

    Task PutSecretStringAsync(
        string secretId,
        string secretString,
        CancellationToken cancellationToken = default);

    Task DeleteSecretAsync(string secretId, CancellationToken cancellationToken = default);
}
