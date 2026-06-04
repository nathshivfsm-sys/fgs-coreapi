namespace Fgs.Setup.Application.Abstractions.Credentials;

/// <summary>
/// Plaintext and KMS-wrapped data encryption key returned from AWS KMS GenerateDataKey.
/// </summary>
public sealed record KmsDataKeyResult(byte[] PlaintextKey, byte[] EncryptedKey, string KeyIdentifier);
