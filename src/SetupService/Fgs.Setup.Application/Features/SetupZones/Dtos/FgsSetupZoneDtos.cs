namespace Fgs.Setup.Application.Features.SetupZones.Dtos;

public sealed record FgsSetupZoneSummaryDto(
    long Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);

public sealed record FgsSetupZoneDetailDto(
    long Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);

public sealed record FgsSetupZoneLookupDto(
    long Id,
    string Code,
    string Name);

public sealed record FgsSetupZoneCreateDto(
    string Code,
    string Name,
    string? Description);

public sealed record FgsSetupZoneUpdateDto(
    string Code,
    string Name,
    string? Description);

public sealed record FgsSetupZonePatchDto(
    string? Code,
    string? Name,
    string? Description,
    bool? IsActive);

public sealed record FgsSetupZoneListFilters(
    string? Code = null,
    string? Name = null);
