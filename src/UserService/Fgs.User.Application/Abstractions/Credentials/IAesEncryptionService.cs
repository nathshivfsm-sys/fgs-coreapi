namespace Fgs.User.Application.Abstractions.Credentials;

/// <summary>
/// AES-256-GCM encryption using a plaintext data encryption key.
/// </summary>
public interface IAesEncryptionService
{
    byte[] Encrypt(byte[] plaintext, byte[] plaintextDataKey);

    byte[] Decrypt(byte[] envelopePayload, byte[] plaintextDataKey);
}
