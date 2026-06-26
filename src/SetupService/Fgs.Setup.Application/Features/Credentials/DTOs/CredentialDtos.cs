using Fgs.Setup.Domain.Enums;

namespace Fgs.Setup.Application.Features.Credentials.DTOs;

public sealed record CredentialSummaryDto(
    CredentialScope Scope,
    string Id,
    string ProviderCode,
    string ProviderName,
    string CredentialName,
    string? Description,
    bool IsActive,
    string? KeyIdentifier);

public sealed record CredentialDetailDto(
    CredentialScope Scope,
    string Id,
    string ProviderCode,
    string ProviderName,
    string CredentialName,
    string? Description,
    bool IsActive,
    string? KeyIdentifier);

public sealed record CredentialSecretDto(
    CredentialScope Scope,
    string Id,
    string ProviderCode,
    string Payload);

public sealed record CredentialMutationResultDto(
    CredentialScope Scope,
    string Id,
    string ProviderCode,
    string CredentialName);
