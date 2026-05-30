namespace Fgs.User.Application.Abstractions.Credentials;

public interface ISecretsManagerService
{
    Task<SecretCreateResult> CreateSecretAsync(
        string secretName,
        string secretJson,
        string kmsKeyArn,
        IReadOnlyDictionary<string, string> tags,
        CancellationToken cancellationToken = default);

    Task<SecretUpdateResult> PutSecretValueAsync(
        string secretArn,
        string secretJson,
        CancellationToken cancellationToken = default);

    Task<string> GetSecretJsonAsync(
        string secretArn,
        string? versionId = null,
        string? versionStage = null,
        CancellationToken cancellationToken = default);

    Task DeleteSecretAsync(string secretArn, bool forceDelete, CancellationToken cancellationToken = default);

    Task RotateSecretAsync(string secretArn, string? rotationLambdaArn, CancellationToken cancellationToken = default);
}

public sealed record SecretCreateResult(string SecretArn, string Name, string VersionId);

public sealed record SecretUpdateResult(string VersionId, string? VersionStages);
