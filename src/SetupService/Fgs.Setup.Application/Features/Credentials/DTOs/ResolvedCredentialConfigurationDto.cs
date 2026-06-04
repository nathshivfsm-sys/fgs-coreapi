namespace Fgs.Setup.Application.Features.Credentials.DTOs;

public sealed record ResolvedCredentialConfigurationDto(
    IReadOnlyDictionary<string, string> Values);
