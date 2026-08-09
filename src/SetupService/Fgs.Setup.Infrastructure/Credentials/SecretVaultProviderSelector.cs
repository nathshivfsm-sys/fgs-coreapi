using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Credentials.Options;

namespace Fgs.Setup.Infrastructure.Credentials;

/// <summary>
/// Resolves which <see cref="ISecretVault"/> implementation to register from options.
/// </summary>
public static class SecretVaultProviderSelector
{
    public static Type ResolveImplementationType(string? defaultVaultProvider) =>
        SecretVaultProviders.IsAwsSecretsManager(defaultVaultProvider)
            ? typeof(AwsSecretsManagerSecretVault)
            : typeof(NoOpSecretVault);

    public static Type ResolveImplementationType(AwsCredentialsOptions options) =>
        ResolveImplementationType(options.DefaultVaultProvider);
}
