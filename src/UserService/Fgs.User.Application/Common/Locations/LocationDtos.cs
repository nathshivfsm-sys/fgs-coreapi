namespace Fgs.User.Application.Common.Locations;

public sealed record LocationWriteDto(
    string? AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string? AddressLine4,
    string? City,
    string? State,
    string? County,
    string? Country,
    string? PostalCode,
    string? FormattedAddress,
    decimal? Latitude,
    decimal? Longitude,
    string? PlaceId);

public sealed record LocationDetailDto(
    Guid Id,
    string? AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string? AddressLine4,
    string? City,
    string? State,
    string? County,
    string? Country,
    string? PostalCode,
    string? FormattedAddress,
    decimal? Latitude,
    decimal? Longitude,
    string? PlaceId,
    bool IsActive);
