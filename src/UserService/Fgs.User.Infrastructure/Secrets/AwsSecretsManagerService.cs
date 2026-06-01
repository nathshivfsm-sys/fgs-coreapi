using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Fgs.User.Application.Abstractions.Credentials;
using Fgs.User.Application.Features.Credentials;
using Microsoft.Extensions.Logging;

namespace Fgs.User.Infrastructure.Secrets;

public sealed class AwsSecretsManagerService : ISecretsManagerService
{
    private const int MaxRetryAttempts = 5;
    private static readonly TimeSpan InitialRetryDelay = TimeSpan.FromMilliseconds(200);

    private readonly IAmazonSecretsManager _client;
    private readonly ILogger<AwsSecretsManagerService> _logger;

    public AwsSecretsManagerService(
        IAmazonSecretsManager client,
        ILogger<AwsSecretsManagerService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public Task<SecretCreateResult> CreateSecretAsync(
        string secretName,
        string secretJson,
        string kmsKeyArn,
        IReadOnlyDictionary<string, string> tags,
        CancellationToken cancellationToken = default) =>
        ExecuteSafeAsync(async ct =>
        {
            var request = new CreateSecretRequest
            {
                Name = secretName,
                SecretString = secretJson,
                KmsKeyId = string.IsNullOrWhiteSpace(kmsKeyArn) ? null : kmsKeyArn,
                Tags = [.. tags.Select(t => new Tag { Key = t.Key, Value = t.Value })]
            };

            var response = await _client.CreateSecretAsync(request, ct);
            _logger.LogInformation("Created secret {SecretName}", secretName);

            return new SecretCreateResult(
                response.ARN ?? string.Empty,
                response.Name ?? secretName,
                response.VersionId ?? string.Empty);
        }, cancellationToken);

    public Task<SecretUpdateResult> PutSecretValueAsync(
        string secretArn,
        string secretJson,
        CancellationToken cancellationToken = default) =>
        ExecuteSafeAsync(async ct =>
        {
            var response = await _client.PutSecretValueAsync(new PutSecretValueRequest
            {
                SecretId = secretArn,
                SecretString = secretJson
            }, ct);

            _logger.LogInformation("Updated secret {SecretArn}", secretArn);

            return new SecretUpdateResult(
                response.VersionId ?? string.Empty,
                response.VersionStages?.FirstOrDefault());
        }, cancellationToken);

    public Task<string> GetSecretJsonAsync(
        string secretArn,
        string? versionId = null,
        string? versionStage = null,
        CancellationToken cancellationToken = default) =>
        ExecuteSafeAsync(async ct =>
        {
            var request = new GetSecretValueRequest { SecretId = secretArn };
            if (!string.IsNullOrWhiteSpace(versionId))
            {
                request.VersionId = versionId;
            }
            else if (!string.IsNullOrWhiteSpace(versionStage))
            {
                request.VersionStage = versionStage;
            }

            var response = await _client.GetSecretValueAsync(request, ct);
            _logger.LogDebug("Fetched secret metadata for {SecretArn}", secretArn);

            return response.SecretString
                ?? throw new InvalidOperationException("Secret value was not returned as a string.");
        }, cancellationToken);

    public Task DeleteSecretAsync(string secretArn, bool forceDelete, CancellationToken cancellationToken = default) =>
        ExecuteSafeAsync(async ct =>
        {
            await _client.DeleteSecretAsync(new DeleteSecretRequest
            {
                SecretId = secretArn,
                ForceDeleteWithoutRecovery = forceDelete
            }, ct);

            _logger.LogInformation("Deleted secret {SecretArn}", secretArn);
            return true;
        }, cancellationToken);

    public Task RotateSecretAsync(
        string secretArn,
        string? rotationLambdaArn,
        CancellationToken cancellationToken = default) =>
        ExecuteSafeAsync(async ct =>
        {
            var request = new RotateSecretRequest { SecretId = secretArn };
            if (!string.IsNullOrWhiteSpace(rotationLambdaArn))
            {
                request.RotationLambdaARN = rotationLambdaArn;
            }

            await _client.RotateSecretAsync(request, ct);
            _logger.LogInformation("Initiated rotation for {SecretArn}", secretArn);
            return true;
        }, cancellationToken);

    private static bool IsTransient(AmazonSecretsManagerException ex) =>
        ex.ErrorCode is "ThrottlingException"
            or "InternalServiceError"
            or "ServiceUnavailable"
            or "InternalError";

    private async Task<T> ExecuteSafeAsync<T>(
        Func<CancellationToken, Task<T>> action,
        CancellationToken cancellationToken)
    {
        var delay = InitialRetryDelay;

        for (var attempt = 1; attempt <= MaxRetryAttempts; attempt++)
        {
            try
            {
                return await action(cancellationToken);
            }
            catch (AmazonSecretsManagerException ex) when (IsTransient(ex) && attempt < MaxRetryAttempts)
            {
                _logger.LogWarning(
                    ex,
                    "Transient AWS Secrets Manager error ({ErrorCode}), retry {Attempt}/{MaxAttempts}",
                    ex.ErrorCode,
                    attempt,
                    MaxRetryAttempts);

                await Task.Delay(delay, cancellationToken);
                delay = TimeSpan.FromMilliseconds(Math.Min(delay.TotalMilliseconds * 2, 5_000));
            }
            catch (AmazonSecretsManagerException ex)
            {
                _logger.LogError(
                    ex,
                    "AWS Secrets Manager operation failed ({ErrorCode}). Message={AwsMessage}",
                    ex.ErrorCode,
                    ex.Message);
                throw MapSecretsManagerException(ex);
            }
        }

        throw new InvalidOperationException("AWS Secrets Manager retry loop exited unexpectedly.");
    }

    private static CredentialSecretsException MapSecretsManagerException(AmazonSecretsManagerException ex)
    {
        var isAccessDenied = ex.ErrorCode is "AccessDeniedException"
            or "AccessDenied"
            or "UnauthorizedOperation";

        if (isAccessDenied)
        {
            return new CredentialSecretsException(
                "AWS IAM principal is not authorized for Secrets Manager. Attach secretsmanager and KMS permissions to the credentials used by this service.",
                ex.ErrorCode,
                isAccessDenied: true,
                ex);
        }

        return new CredentialSecretsException(
            "Credential vault operation failed.",
            ex.ErrorCode,
            innerException: ex);
    }
}
