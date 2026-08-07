using Fgs.Setup.Application.Abstractions.Credentials;

namespace Fgs.Setup.Infrastructure.Credentials;

/// <summary>
/// No-op vault used when <see cref="SecretVaultProviders.Database"/> is selected.
/// Credential payloads stay in DB + KMS; this type is registered so DI always has an <see cref="ISecretVault"/>.
/// </summary>
public sealed class NoOpSecretVault : ISecretVault
{
    public string ProviderName => SecretVaultProviders.Database;

    public Task<string?> GetSecretStringAsync(string secretId, CancellationToken cancellationToken = default) =>
        Task.FromResult<string?>(null);

    public Task CreateSecretAsync(
        string secretId,
        string secretString,
        string? description = null,
        CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task PutSecretStringAsync(
        string secretId,
        string secretString,
        CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task DeleteSecretAsync(string secretId, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;
}
