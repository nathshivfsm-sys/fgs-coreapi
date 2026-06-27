namespace Fgs.Setup.Application.Abstractions.Credentials;

/// <summary>
/// AWS KMS operations for envelope encryption.
/// </summary>
public interface IKmsService
{
    Task<KmsDataKeyResult> GenerateDataKeyAsync(CancellationToken cancellationToken = default);

    Task<byte[]> DecryptDataKeyAsync(byte[] encryptedDataKey, CancellationToken cancellationToken = default);

    Task<byte[]> ReEncryptDataKeyAsync(
        byte[] encryptedDataKey,
        string? sourceKeyIdentifier,
        CancellationToken cancellationToken = default);

    Task<(byte[] EncryptedDataKey, string KeyIdentifier)> ReEncryptDataKeyWithMetadataAsync(
        byte[] encryptedDataKey,
        string? sourceKeyIdentifier,
        CancellationToken cancellationToken = default);
}
