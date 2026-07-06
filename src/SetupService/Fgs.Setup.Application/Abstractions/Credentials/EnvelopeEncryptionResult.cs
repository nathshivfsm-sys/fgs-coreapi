namespace Fgs.Setup.Application.Abstractions.Credentials;

/// <summary>
/// Encrypted credential payload and associated KMS envelope metadata.
/// </summary>
public sealed record EnvelopeEncryptionResult(
    byte[] CredentialData,
    byte[] EncryptedDataKey);
