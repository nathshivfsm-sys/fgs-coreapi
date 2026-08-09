namespace Fgs.Setup.Application.Features.JobTypes.Dtos;

public sealed record JobTypeSummaryDto(
    long Id,
    string JobTypeCode,
    string Name,
    short UsedFor,
    string? BusinessUnit,
    string? BackgroundColor,
    string? TextColor,
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
    string? BackgroundColor,
    string? TextColor,
    bool ShowToFieldTech,
    bool ShowOnCustomerPortal,
    short? DisplayOrder,
    bool IsActive);

public sealed record JobTypeLookupDto(
    long Id,
    string JobTypeCode,
    string Name,
    short? DisplayOrder);

public sealed record JobTypeCreateDto(
    string JobTypeCode,
    string Name,
    short UsedFor,
    string? BusinessUnit,
    string? BackgroundColor,
    string? TextColor,
    bool ShowToFieldTech,
    bool ShowOnCustomerPortal,
    short? DisplayOrder);

public sealed record JobTypeUpdateDto(
    string JobTypeCode,
    string Name,
    short UsedFor,
    string? BusinessUnit,
    string? BackgroundColor,
    string? TextColor,
    bool ShowToFieldTech,
    bool ShowOnCustomerPortal,
    short? DisplayOrder);

public sealed record JobTypePatchDto(
    string? JobTypeCode,
    string? Name,
    short? UsedFor,
    string? BusinessUnit,
    string? BackgroundColor,
    string? TextColor,
    bool? ShowToFieldTech,
    bool? ShowOnCustomerPortal,
    short? DisplayOrder,
    bool? IsActive);

public sealed record JobTypeListFilters(
    string? JobTypeCode = null,
    string? Name = null,
    short? UsedFor = null);
