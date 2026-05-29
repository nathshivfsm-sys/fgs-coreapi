namespace Fgs.User.Application.Features.Credentials.Models;

public sealed record CredentialSecretResolution(
    Guid SecretId,
    string ProviderTypeCode,
    string SecretJson,
    int VersionNo);
