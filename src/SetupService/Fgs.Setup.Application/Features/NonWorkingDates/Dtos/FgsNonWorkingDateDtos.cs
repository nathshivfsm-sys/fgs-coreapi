namespace Fgs.Setup.Application.Features.NonWorkingDates.Dtos;

public sealed record FgsNonWorkingDateSummaryDto(
    long Id,
    DateOnly NonWorkingDate,
    string Name,
    bool IsActive);

public sealed record FgsNonWorkingDateDetailDto(
    long Id,
    DateOnly NonWorkingDate,
    string Name,
    bool IsActive);

public sealed record FgsNonWorkingDateLookupDto(
    long Id,
    DateOnly NonWorkingDate,
    string Name);

public sealed record FgsNonWorkingDateCreateDto(
    DateOnly NonWorkingDate,
    string Name);

public sealed record FgsNonWorkingDateUpdateDto(
    DateOnly NonWorkingDate,
    string Name);

public sealed record FgsNonWorkingDatePatchDto(
    DateOnly? NonWorkingDate = null,
    string? Name = null,
    bool? IsActive = null);

public sealed record FgsNonWorkingDateListFilters(
    DateOnly? NonWorkingDate = null,
    string? Name = null);
