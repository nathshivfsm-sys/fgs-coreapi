namespace Fgs.User.Application.Features.ApiEvents.Dtos;

public sealed record FgsApiEventSummaryDto(
    long Id,
    string EventCode,
    string EventCategory,
    string Name,
    string? Description,
    short EventVersion,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsApiEventDetailDto(
    long Id,
    string EventCode,
    string EventCategory,
    string Name,
    string? Description,
    short EventVersion,
    short DisplayOrder,
    bool IsActive);

public sealed record FgsApiEventLookupDto(
    long Id,
    string EventCode,
    string EventCategory,
    string Name,
    short EventVersion,
    short DisplayOrder);

public sealed record FgsApiEventCreateDto(
    string EventCode,
    string EventCategory,
    string Name,
    string? Description = null,
    short EventVersion = 1,
    short DisplayOrder = 1);

public sealed record FgsApiEventUpdateDto(
    string EventCode,
    string EventCategory,
    string Name,
    string? Description,
    short EventVersion,
    short DisplayOrder);

public sealed record FgsApiEventPatchDto(
    string? EventCode,
    string? EventCategory,
    string? Name,
    string? Description,
    short? EventVersion,
    short? DisplayOrder,
    bool? IsActive);

public sealed record FgsApiEventListFilters(
    string? EventCode = null,
    string? EventCategory = null,
    string? Name = null);
