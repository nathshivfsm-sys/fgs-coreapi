namespace Fgs.Setup.Application.Features.ResolutionCodes.Dtos;

public sealed record ResolutionCodeSummaryDto(
    long Id,
    int GloResolutionTypeId,
    string ResolutionCode,
    string ResolutionName,
    bool IsMobileVisible,
    bool IsActive);

public sealed record ResolutionCodeDetailDto(
    long Id,
    int GloResolutionTypeId,
    string ResolutionCode,
    string ResolutionName,
    bool IsMobileVisible,
    bool IsActive);

public sealed record ResolutionCodeLookupDto(
    long Id,
    string ResolutionCode,
    string ResolutionName);

public sealed record ResolutionCodeCreateDto(
    int GloResolutionTypeId,
    string ResolutionCode,
    string ResolutionName,
    bool IsMobileVisible);

public sealed record ResolutionCodeUpdateDto(
    int GloResolutionTypeId,
    string ResolutionCode,
    string ResolutionName,
    bool IsMobileVisible);

public sealed record ResolutionCodePatchDto(
    int? GloResolutionTypeId,
    string? ResolutionCode,
    string? ResolutionName,
    bool? IsMobileVisible,
    bool? IsActive);

public sealed record ResolutionCodeListFilters(
    string? ResolutionCode = null,
    string? ResolutionName = null);
