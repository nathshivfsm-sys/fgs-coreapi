using Fgs.User.Domain.Enums;

namespace Fgs.User.Application.Features.Credentials.DTOs;

public sealed record CredentialSummaryDto(
    CredentialScope Scope,
    string Id,
    string ProviderCode,
    string ProviderName,
    string CredentialName,
    string? Description,
    bool IsActive,
    string? KeyIdentifier,
    long? TenantId,
    long? CompanyId,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record CredentialDetailDto(
    CredentialScope Scope,
    string Id,
    string ProviderCode,
    string ProviderName,
    string CredentialName,
    string? Description,
    bool IsActive,
    string? KeyIdentifier,
    long? TenantId,
    long? CompanyId,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

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
