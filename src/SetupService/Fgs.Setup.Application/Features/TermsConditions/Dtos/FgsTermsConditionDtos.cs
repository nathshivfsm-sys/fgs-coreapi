namespace Fgs.Setup.Application.Features.TermsConditions.Dtos;

public sealed record FgsTermsConditionSummaryDto(
    long Id,
    string Code,
    string Name,
    int VersionNumber,
    bool IsActive);

public sealed record FgsTermsConditionDetailDto(
    long Id,
    string Code,
    string Name,
    int VersionNumber,
    string TermsText,
    bool IsActive);

public sealed record FgsTermsConditionLookupDto(
    long Id,
    string Code,
    string Name,
    int VersionNumber);

public sealed record FgsTermsConditionCreateDto(
    string Code,
    string Name,
    int VersionNumber,
    string TermsText);

public sealed record FgsTermsConditionUpdateDto(
    string Code,
    string Name,
    int VersionNumber,
    string TermsText);

public sealed record FgsTermsConditionPatchDto(
    string? Code,
    string? Name,
    int? VersionNumber,
    string? TermsText,
    bool? IsActive);

public sealed record FgsTermsConditionListFilters(
    string? Code = null,
    string? Name = null);
