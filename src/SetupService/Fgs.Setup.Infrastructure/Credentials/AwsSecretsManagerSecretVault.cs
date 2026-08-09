using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Fgs.Setup.Application.Abstractions.Credentials;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Fgs.Credentials.Options;

namespace Fgs.Setup.Infrastructure.Credentials;

/// <summary>
/// AWS Secrets Manager-backed vault. Only constructed when
/// <c>AwsCredentials:DefaultVaultProvider</c> is <see cref="SecretVaultProviders.AwsSecretsManager"/>.
/// Secret name pattern: <c>{Environment}/{ApplicationSlug}/{tenantCode}/{providerCode}</c>.
/// </summary>
public sealed class AwsSecretsManagerSecretVault : ISecretVault
{
    private readonly IAmazonSecretsManager _client;
    private readonly AwsCredentialsOptions _options;
    private readonly ILogger<AwsSecretsManagerSecretVault> _logger;

    public AwsSecretsManagerSecretVault(
        IAmazonSecretsManager client,
        IOptions<AwsCredentialsOptions> options,
        ILogger<AwsSecretsManagerSecretVault> logger)
    {
        _client = client;
        _options = options.Value;
        _logger = logger;
    }

    public string ProviderName => SecretVaultProviders.AwsSecretsManager;

    public async Task<string?> GetSecretStringAsync(string secretId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.GetSecretValueAsync(
                new GetSecretValueRequest { SecretId = secretId },
                cancellationToken);
            return response.SecretString;
        }
        catch (ResourceNotFoundException)
        {
            _logger.LogDebug("Secret {SecretId} was not found in Secrets Manager.", secretId);
            return null;
        }
    }

    public async Task CreateSecretAsync(
        string secretId,
        string secretString,
        string? description = null,
        CancellationToken cancellationToken = default)
    {
        var request = new CreateSecretRequest
        {
            Name = secretId,
            SecretString = secretString,
            Description = description
        };

        if (!string.IsNullOrWhiteSpace(_options.KmsKeyArn))
        {
            request.KmsKeyId = _options.KmsKeyArn;
        }

        await _client.CreateSecretAsync(request, cancellationToken);
        _logger.LogInformation("Created secret {SecretId} in Secrets Manager.", secretId);
    }

    public async Task PutSecretStringAsync(
        string secretId,
        string secretString,
        CancellationToken cancellationToken = default)
    {
        await _client.PutSecretValueAsync(
            new PutSecretValueRequest
            {
                SecretId = secretId,
                SecretString = secretString
            },
            cancellationToken);
        _logger.LogInformation("Updated secret {SecretId} in Secrets Manager.", secretId);
    }

    public async Task DeleteSecretAsync(string secretId, CancellationToken cancellationToken = default)
    {
        await _client.DeleteSecretAsync(
            new DeleteSecretRequest
            {
                SecretId = secretId,
                ForceDeleteWithoutRecovery = false
            },
            cancellationToken);
        _logger.LogInformation("Scheduled deletion for secret {SecretId} in Secrets Manager.", secretId);
    }
}
