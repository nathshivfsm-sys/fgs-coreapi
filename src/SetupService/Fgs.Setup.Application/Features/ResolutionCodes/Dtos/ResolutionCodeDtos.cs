namespace Fgs.Setup.Application.Features.ResolutionCodes.Dtos;

public sealed record ResolutionCodeSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    int GloResolutionTypeId,
    string ResolutionCode,
    string ResolutionName,
    bool IsMobileVisible,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record ResolutionCodeDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    int GloResolutionTypeId,
    string ResolutionCode,
    string ResolutionName,
    bool IsMobileVisible,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

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
