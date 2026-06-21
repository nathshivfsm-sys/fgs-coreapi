namespace Fgs.Setup.Application.Features.LeadSources.Dtos;

public sealed record LeadSourceSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string SourceCode,
    string SourceName,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record LeadSourceDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string SourceCode,
    string SourceName,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record LeadSourceLookupDto(
    long Id,
    string SourceCode,
    string SourceName);

public sealed record LeadSourceCreateDto(
    string SourceCode,
    string SourceName,
    string? Description);

public sealed record LeadSourceUpdateDto(
    string SourceCode,
    string SourceName,
    string? Description);

public sealed record LeadSourcePatchDto(
    string? SourceCode,
    string? SourceName,
    string? Description,
    bool? IsActive);

public sealed record LeadSourceListFilters(
    string? SourceCode = null,
    string? SourceName = null);
