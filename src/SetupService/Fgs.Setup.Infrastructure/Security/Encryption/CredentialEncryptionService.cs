using Fgs.Setup.Application.Abstractions.Credentials;

namespace Fgs.Setup.Infrastructure.Security.Encryption;

public sealed class CredentialEncryptionService : ICredentialEncryptionService
{
    private readonly IKmsService _kmsService;
    private readonly IAesEncryptionService _aesEncryptionService;

    public CredentialEncryptionService(IKmsService kmsService, IAesEncryptionService aesEncryptionService)
    {
        _kmsService = kmsService;
        _aesEncryptionService = aesEncryptionService;
    }

    public async Task<EnvelopeEncryptionResult> EncryptAsync(byte[] plaintext, CancellationToken cancellationToken = default)
    {
        var dataKey = await _kmsService.GenerateDataKeyAsync(cancellationToken);
        try
        {
            var credentialData = _aesEncryptionService.Encrypt(plaintext, dataKey.PlaintextKey);
            return new EnvelopeEncryptionResult(credentialData, dataKey.EncryptedKey);
        }
        finally
        {
            CryptographicBufferWipe.Wipe(dataKey.PlaintextKey);
        }
    }

    public async Task<byte[]> DecryptAsync(
        byte[] credentialData,
        byte[] encryptedDataKey,
        CancellationToken cancellationToken = default)
    {
        var plaintextKey = await _kmsService.DecryptDataKeyAsync(encryptedDataKey, cancellationToken);
        try
        {
            return _aesEncryptionService.Decrypt(credentialData, plaintextKey);
        }
        finally
        {
            CryptographicBufferWipe.Wipe(plaintextKey);
        }
    }

    public async Task<EnvelopeEncryptionResult> ReEncryptPayloadAsync(
        byte[] credentialData,
        byte[] encryptedDataKey,
        CancellationToken cancellationToken = default)
    {
        var plaintext = await DecryptAsync(credentialData, encryptedDataKey, cancellationToken);
        return await EncryptAsync(plaintext, cancellationToken);
    }

    public Task<byte[]> ReEncryptDataKeyOnlyAsync(
        byte[] encryptedDataKey,
        string? sourceKeyIdentifier,
        CancellationToken cancellationToken = default) =>
        _kmsService.ReEncryptDataKeyAsync(encryptedDataKey, sourceKeyIdentifier, cancellationToken);
}

internal static class CryptographicBufferWipe
{
    public static void Wipe(byte[] buffer) => Array.Clear(buffer, 0, buffer.Length);
}
