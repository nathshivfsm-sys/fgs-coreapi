namespace Fgs.Setup.Application.Features.SetupTechSkillLevels.Dtos;

public sealed record FgsSetupTechSkillLevelSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    string Code,
    string Name,
    string? Description,
    int? SortOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record FgsSetupTechSkillLevelDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    string Code,
    string Name,
    string? Description,
    int? SortOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsSetupTechSkillLevelLookupDto(
    long Id,
    string Code,
    string Name,
    int? SortOrder);

public sealed record FgsSetupTechSkillLevelCreateDto(
    string Code,
    string Name,
    string? Description,
    int? SortOrder);

public sealed record FgsSetupTechSkillLevelUpdateDto(
    string Code,
    string Name,
    string? Description,
    int? SortOrder);

public sealed record FgsSetupTechSkillLevelPatchDto(
    string? Code,
    string? Name,
    string? Description,
    int? SortOrder,
    bool? IsActive);

public sealed record FgsSetupTechSkillLevelListFilters(
    string? Code = null,
    string? Name = null);
