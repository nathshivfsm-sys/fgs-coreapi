namespace Fgs.User.Application.Abstractions.Credentials;

/// <summary>
/// High-level envelope encryption for credential secrets.
/// </summary>
public interface ICredentialEncryptionService
{
    Task<EnvelopeEncryptionResult> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default);

    Task<byte[]> DecryptAsync(
        byte[] credentialData,
        byte[] encryptedDataKey,
        CancellationToken cancellationToken = default);

    Task<EnvelopeEncryptionResult> ReEncryptPayloadAsync(
        byte[] credentialData,
        byte[] encryptedDataKey,
        CancellationToken cancellationToken = default);

    Task<(byte[] EncryptedDataKey, string KeyIdentifier)> ReEncryptDataKeyOnlyAsync(
        byte[] encryptedDataKey,
        string? sourceKeyIdentifier,
        CancellationToken cancellationToken = default);
}
