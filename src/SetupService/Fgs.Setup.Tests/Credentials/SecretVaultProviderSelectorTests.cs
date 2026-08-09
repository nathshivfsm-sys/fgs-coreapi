using Fgs.Setup.Application.Abstractions.Credentials;
using Fgs.Setup.Infrastructure.Credentials;
using Fgs.Credentials.Options;

namespace Fgs.Setup.Tests.Credentials;

public sealed class SecretVaultProviderSelectorTests
{
    [Theory]
    [InlineData(null, typeof(NoOpSecretVault))]
    [InlineData("", typeof(NoOpSecretVault))]
    [InlineData("Database", typeof(NoOpSecretVault))]
    [InlineData("database", typeof(NoOpSecretVault))]
    [InlineData("AWS", typeof(NoOpSecretVault))]
    [InlineData(SecretVaultProviders.AwsSecretsManager, typeof(AwsSecretsManagerSecretVault))]
    [InlineData("awssecretsmanager", typeof(AwsSecretsManagerSecretVault))]
    public void ResolveImplementationType_SelectsExpectedVault(string? provider, Type expected)
    {
        SecretVaultProviderSelector.ResolveImplementationType(provider).Should().Be(expected);
    }

    [Fact]
    public void ResolveImplementationType_FromOptions_UsesDefaultVaultProvider()
    {
        var options = new AwsCredentialsOptions { DefaultVaultProvider = SecretVaultProviders.Database };

        SecretVaultProviderSelector.ResolveImplementationType(options)
            .Should().Be(typeof(NoOpSecretVault));

        options.DefaultVaultProvider = SecretVaultProviders.AwsSecretsManager;

        SecretVaultProviderSelector.ResolveImplementationType(options)
            .Should().Be(typeof(AwsSecretsManagerSecretVault));
    }

    [Fact]
    public void AwsCredentialsOptions_DefaultVaultProvider_IsDatabase()
    {
        new AwsCredentialsOptions().DefaultVaultProvider.Should().Be(SecretVaultProviders.Database);
    }

    [Fact]
    public async Task NoOpSecretVault_IsUnusedSafeDefault()
    {
        var vault = new NoOpSecretVault();

        vault.ProviderName.Should().Be(SecretVaultProviders.Database);
        (await vault.GetSecretStringAsync("any")).Should().BeNull();
        await vault.Invoking(v => v.CreateSecretAsync("any", "{}")).Should().NotThrowAsync();
        await vault.Invoking(v => v.PutSecretStringAsync("any", "{}")).Should().NotThrowAsync();
        await vault.Invoking(v => v.DeleteSecretAsync("any")).Should().NotThrowAsync();
    }
}
