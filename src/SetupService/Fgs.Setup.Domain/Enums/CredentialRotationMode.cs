namespace Fgs.Setup.Domain.Enums;

/// <summary>
/// Controls how credential key material is rotated.
/// </summary>
public enum CredentialRotationMode
{
    /// <summary>
    /// Re-encrypts the payload with a newly generated data encryption key (full rotation).
    /// </summary>
    Full = 1,

    /// <summary>
    /// Re-wraps the existing encrypted data key with the current KMS master key (KMS key rotation).
    /// </summary>
    KmsReEncrypt = 2
}
