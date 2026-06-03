namespace Fgs.User.Application.Features.Credentials.DTOs;

public sealed record ResolvedCredentialConfigurationDto(
    IReadOnlyDictionary<string, string> Values);
