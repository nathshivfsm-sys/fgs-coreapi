using Fgs.User.Domain.Entities;

namespace Fgs.User.Application.Credentials;

/// <summary>
/// Maps existing <see cref="FgsCredentialSecret"/> columns to AWS Secrets Manager metadata.
/// No schema changes: values are references only, never ciphertext or secret payloads.
/// </summary>
public static class CredentialSecretStorageMapping
{
    public static string GetAwsSecretArn(FgsCredentialSecret secret) => secret.EncryptedSecretValue;

    public static void SetAwsSecretArn(FgsCredentialSecret secret, string awsSecretArn) =>
        secret.EncryptedSecretValue = awsSecretArn;

    public static string GetRegionName(FgsCredentialSecret secret) => secret.EncryptedDek;

    public static void SetRegionName(FgsCredentialSecret secret, string regionName) =>
        secret.EncryptedDek = regionName;

    public static string GetKmsKeyArn(FgsCredentialSecret secret) => secret.EncryptionKeyId;

    public static void SetKmsKeyArn(FgsCredentialSecret secret, string kmsKeyArn) =>
        secret.EncryptionKeyId = kmsKeyArn;
}
