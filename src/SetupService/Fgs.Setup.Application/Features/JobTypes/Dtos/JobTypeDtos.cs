namespace Fgs.Setup.Application.Features.JobTypes.Dtos;

public sealed record JobTypeSummaryDto(
    long Id,
    string JobTypeCode,
    string Name,
    short UsedFor,
    string? BusinessUnit,
    bool ShowToFieldTech,
    bool ShowOnCustomerPortal,
    short? DisplayOrder,
    bool IsActive);

public sealed record JobTypeDetailDto(
    long Id,
    string JobTypeCode,
    string Name,
    short UsedFor,
    string? BusinessUnit,
    bool ShowToFieldTech,
    bool ShowOnCustomerPortal,
    short? DisplayOrder,
    bool IsActive,
    IReadOnlyList<JobTypeSubCategoryDto> SubCategories);

public sealed record JobTypeLookupDto(
    long Id,
    string JobTypeCode,
    string Name,
    short? DisplayOrder);

public sealed record JobTypeSubCategoryDto(
    long Id,
    long JobTypeTaskId,
    short DisplayOrder,
    bool IsActive);

public sealed record JobTypeSubCategoryWriteDto(
    long JobTypeTaskId,
    short? DisplayOrder,
    bool IsActive = true);

public sealed record JobTypeCreateDto(
    string JobTypeCode,
    string Name,
    short UsedFor,
    string? BusinessUnit,
    bool ShowToFieldTech,
    bool ShowOnCustomerPortal,
    short? DisplayOrder,
    IReadOnlyList<JobTypeSubCategoryWriteDto>? SubCategories = null,
    bool IsActive = true);

public sealed record JobTypeUpdateDto(
    string JobTypeCode,
    string Name,
    short UsedFor,
    string? BusinessUnit,
    bool ShowToFieldTech,
    bool ShowOnCustomerPortal,
    short? DisplayOrder,
    IReadOnlyList<JobTypeSubCategoryWriteDto>? SubCategories = null);

public sealed record JobTypePatchDto(
    string? JobTypeCode,
    string? Name,
    short? UsedFor,
    string? BusinessUnit,
    bool? ShowToFieldTech,
    bool? ShowOnCustomerPortal,
    short? DisplayOrder,
    bool? IsActive,
    IReadOnlyList<JobTypeSubCategoryWriteDto>? SubCategories = null);

public sealed record JobTypeListFilters(
    string? JobTypeCode = null,
    string? Name = null,
    short? UsedFor = null);
