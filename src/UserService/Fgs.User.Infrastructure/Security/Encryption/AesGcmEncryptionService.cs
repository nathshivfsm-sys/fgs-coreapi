using System.Security.Cryptography;
using Fgs.User.Application.Abstractions.Credentials;

namespace Fgs.User.Infrastructure.Security.Encryption;

/// <summary>
/// AES-256-GCM envelope format: [version:1][nonce:12][tag:16][ciphertext:*].
/// </summary>
public sealed class AesGcmEncryptionService : IAesEncryptionService
{
    private const byte FormatVersion = 1;
    private const int NonceSize = 12;
    private const int TagSize = 16;
    private const int KeySize = 32;

    public byte[] Encrypt(byte[] plaintext, byte[] plaintextDataKey)
    {
        ValidateKey(plaintextDataKey);

        var nonce = RandomNumberGenerator.GetBytes(NonceSize);
        var ciphertext = new byte[plaintext.Length];
        var tag = new byte[TagSize];

        using var aes = new AesGcm(plaintextDataKey, TagSize);
        aes.Encrypt(nonce, plaintext, ciphertext, tag);

        var envelope = new byte[1 + NonceSize + TagSize + ciphertext.Length];
        envelope[0] = FormatVersion;
        Buffer.BlockCopy(nonce, 0, envelope, 1, NonceSize);
        Buffer.BlockCopy(tag, 0, envelope, 1 + NonceSize, TagSize);
        Buffer.BlockCopy(ciphertext, 0, envelope, 1 + NonceSize + TagSize, ciphertext.Length);
        return envelope;
    }

    public byte[] Decrypt(byte[] envelopePayload, byte[] plaintextDataKey)
    {
        ValidateKey(plaintextDataKey);

        if (envelopePayload.Length < 1 + NonceSize + TagSize + 1)
        {
            throw new CryptographicException("Credential envelope payload is invalid.");
        }

        if (envelopePayload[0] != FormatVersion)
        {
            throw new CryptographicException($"Unsupported credential envelope version '{envelopePayload[0]}'.");
        }

        var nonce = envelopePayload.AsSpan(1, NonceSize);
        var tag = envelopePayload.AsSpan(1 + NonceSize, TagSize);
        var ciphertext = envelopePayload.AsSpan(1 + NonceSize + TagSize);

        var plaintext = new byte[ciphertext.Length];
        using var aes = new AesGcm(plaintextDataKey, TagSize);
        aes.Decrypt(nonce, ciphertext, tag, plaintext);
        return plaintext;
    }

    private static void ValidateKey(byte[] plaintextDataKey)
    {
        if (plaintextDataKey.Length != KeySize)
        {
            throw new CryptographicException("AES-256-GCM requires a 256-bit data encryption key.");
        }
    }
}
