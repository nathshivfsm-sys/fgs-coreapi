namespace Fgs.Setup.Application.Features.SetupLaborRateTypes.Dtos;

public sealed record FgsSetupLaborRateTypeSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string Name,
    string? Description,
    int SortOrder,
    bool IsSystem,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsSetupLaborRateTypeDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string Name,
    string? Description,
    int SortOrder,
    bool IsSystem,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsSetupLaborRateTypeLookupDto(
    long Id,
    string Name,
    int SortOrder);

public sealed record FgsSetupLaborRateTypeCreateDto(
    string Name,
    string? Description,
    int SortOrder,
    bool IsSystem);

public sealed record FgsSetupLaborRateTypeUpdateDto(
    string Name,
    string? Description,
    int SortOrder,
    bool IsSystem);

public sealed record FgsSetupLaborRateTypePatchDto(
    string? Name,
    string? Description,
    int? SortOrder,
    bool? IsSystem,
    bool? IsActive);

public sealed record FgsSetupLaborRateTypeListFilters(
    string? Name = null);
