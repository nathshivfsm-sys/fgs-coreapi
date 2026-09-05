namespace Fgs.Setup.Application.Features.EntityDefaultTermsConditions.Dtos;

public sealed record FgsEntityDefaultTermsConditionSummaryDto(
    long Id,
    string EntityType,
    long TermsConditionId,
    string? TermsConditionCode,
    string? TermsConditionName,
    int? TermsConditionVersionNumber,
    bool IsActive);

public sealed record FgsEntityDefaultTermsConditionDetailDto(
    long Id,
    string EntityType,
    long TermsConditionId,
    string? TermsConditionCode,
    string? TermsConditionName,
    int? TermsConditionVersionNumber,
    bool IsActive);

public sealed record FgsEntityDefaultTermsConditionLookupDto(
    long Id,
    string EntityType,
    long TermsConditionId);

public sealed record FgsEntityDefaultTermsConditionCreateDto(
    string EntityType,
    long TermsConditionId);

public sealed record FgsEntityDefaultTermsConditionUpdateDto(
    string EntityType,
    long TermsConditionId);

public sealed record FgsEntityDefaultTermsConditionPatchDto(
    string? EntityType,
    long? TermsConditionId,
    bool? IsActive);

public sealed record FgsEntityDefaultTermsConditionListFilters(
    string? EntityType = null,
    long? TermsConditionId = null);
