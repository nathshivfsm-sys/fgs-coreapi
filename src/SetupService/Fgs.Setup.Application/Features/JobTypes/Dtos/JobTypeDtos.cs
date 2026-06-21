namespace Fgs.Setup.Application.Features.JobTypes.Dtos;

public sealed record JobTypeSummaryDto(
    long Id,
    long TenantId,
    long CompanyId,
    long JobTypeCategoryId,
    long? JobTypeSubCategoryId,
    string JobTypeCode,
    string TaskName,
    string? Description,
    string UsedFor,
    string? Trade,
    int? EstimatedDurationMinutes,
    string? BusinessUnit,
    short Priority,
    string? BackgroundColor,
    string? TextColor,
    bool ShowToFieldTech,
    bool ShowOnCustomerPortal,
    short? DisplayOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    DateTimeOffset? UpdatedOn);

public sealed record JobTypeDetailDto(
    long Id,
    long TenantId,
    long CompanyId,
    long JobTypeCategoryId,
    long? JobTypeSubCategoryId,
    string JobTypeCode,
    string TaskName,
    string? Description,
    string UsedFor,
    string? Trade,
    int? EstimatedDurationMinutes,
    string? BusinessUnit,
    short Priority,
    string? BackgroundColor,
    string? TextColor,
    bool ShowToFieldTech,
    bool ShowOnCustomerPortal,
    short? DisplayOrder,
    bool IsActive,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record JobTypeLookupDto(
    long Id,
    string JobTypeCode,
    string TaskName,
    short? DisplayOrder);

public sealed record JobTypeCreateDto(
    long JobTypeCategoryId,
    long? JobTypeSubCategoryId,
    string JobTypeCode,
    string TaskName,
    string? Description,
    string UsedFor,
    string? Trade,
    int? EstimatedDurationMinutes,
    string? BusinessUnit,
    short Priority,
    string? BackgroundColor,
    string? TextColor,
    bool ShowToFieldTech,
    bool ShowOnCustomerPortal,
    short? DisplayOrder);

public sealed record JobTypeUpdateDto(
    long JobTypeCategoryId,
    long? JobTypeSubCategoryId,
    string JobTypeCode,
    string TaskName,
    string? Description,
    string UsedFor,
    string? Trade,
    int? EstimatedDurationMinutes,
    string? BusinessUnit,
    short Priority,
    string? BackgroundColor,
    string? TextColor,
    bool ShowToFieldTech,
    bool ShowOnCustomerPortal,
    short? DisplayOrder);

public sealed record JobTypePatchDto(
    long? JobTypeCategoryId,
    long? JobTypeSubCategoryId,
    string? JobTypeCode,
    string? TaskName,
    string? Description,
    string? UsedFor,
    string? Trade,
    int? EstimatedDurationMinutes,
    string? BusinessUnit,
    short? Priority,
    string? BackgroundColor,
    string? TextColor,
    bool? ShowToFieldTech,
    bool? ShowOnCustomerPortal,
    short? DisplayOrder,
    bool? IsActive);

public sealed record JobTypeListFilters(
    string? JobTypeCode = null,
    string? TaskName = null);
