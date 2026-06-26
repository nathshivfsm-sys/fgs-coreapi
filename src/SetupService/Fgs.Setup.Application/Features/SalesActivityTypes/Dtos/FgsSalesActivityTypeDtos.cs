namespace Fgs.Setup.Application.Features.SalesActivityTypes.Dtos;

public sealed record FgsSalesActivityTypeSummaryDto(
    long Id,
    string ActivityTypeCode,
    string ActivityTypeName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    bool AllowManualSelection,
    bool IsActive);

public sealed record FgsSalesActivityTypeDetailDto(
    long Id,
    string ActivityTypeCode,
    string ActivityTypeName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    bool AllowManualSelection,
    bool IsActive);

public sealed record FgsSalesActivityTypeLookupDto(
    long Id,
    string ActivityTypeCode,
    string ActivityTypeName,
    short DisplayOrder);

public sealed record FgsSalesActivityTypeCreateDto(
    string ActivityTypeCode,
    string ActivityTypeName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    bool AllowManualSelection);

public sealed record FgsSalesActivityTypeUpdateDto(
    string ActivityTypeCode,
    string ActivityTypeName,
    string? Description,
    short DisplayOrder,
    bool IsSystem,
    bool AppliesToLead,
    bool AppliesToOpportunity,
    bool AllowManualSelection);

public sealed record FgsSalesActivityTypePatchDto(
    string? ActivityTypeCode,
    string? ActivityTypeName,
    string? Description,
    short? DisplayOrder,
    bool? IsSystem,
    bool? AppliesToLead,
    bool? AppliesToOpportunity,
    bool? AllowManualSelection,
    bool? IsActive);

public sealed record FgsSalesActivityTypeListFilters(
    string? ActivityTypeCode = null,
    string? ActivityTypeName = null);
