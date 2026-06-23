using Fgs.User.Domain.Entities;

namespace Fgs.User.Application.Common.Locations;

public static class LocationMapper
{
    public static LocationDetailDto? ToDetailDto(FgsLocation? location) =>
        location is null
            ? null
            : new LocationDetailDto(
                location.Id,
                location.AddressLine1,
                location.AddressLine2,
                location.AddressLine3,
                location.AddressLine4,
                location.City,
                location.State,
                location.County,
                location.Country,
                location.PostalCode,
                location.FormattedAddress,
                location.Latitude,
                location.Longitude,
                location.PlaceId,
                location.IsActive,
                location.CreatedOn,
                location.CreatedBy,
                location.UpdatedOn,
                location.UpdatedBy);

    public static void ApplyWriteDto(FgsLocation location, LocationWriteDto dto, DateTimeOffset now)
    {
        location.AddressLine1 = TrimOrNull(dto.AddressLine1);
        location.AddressLine2 = TrimOrNull(dto.AddressLine2);
        location.AddressLine3 = TrimOrNull(dto.AddressLine3);
        location.AddressLine4 = TrimOrNull(dto.AddressLine4);
        location.City = TrimOrNull(dto.City);
        location.State = TrimOrNull(dto.State);
        location.County = TrimOrNull(dto.County);
        location.Country = TrimOrNull(dto.Country);
        location.PostalCode = TrimOrNull(dto.PostalCode);
        location.FormattedAddress = TrimOrNull(dto.FormattedAddress)
            ?? FormatAddress(
                location.AddressLine1,
                location.AddressLine2,
                location.City,
                location.State,
                location.PostalCode,
                location.Country);
        location.Latitude = dto.Latitude;
        location.Longitude = dto.Longitude;
        location.PlaceId = TrimOrNull(dto.PlaceId);
        location.UpdatedOn = now;
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static string? FormatAddress(
        string? line1,
        string? line2,
        string? city,
        string? state,
        string? postalCode,
        string? country)
    {
        if (string.IsNullOrWhiteSpace(line1)
            || string.IsNullOrWhiteSpace(city)
            || string.IsNullOrWhiteSpace(state)
            || string.IsNullOrWhiteSpace(postalCode))
        {
            return null;
        }

        var parts = new List<string> { line1.Trim() };
        if (!string.IsNullOrWhiteSpace(line2))
        {
            parts.Add(line2.Trim());
        }

        parts.Add($"{city.Trim()}, {state.Trim()} {postalCode.Trim()}");
        if (!string.IsNullOrWhiteSpace(country))
        {
            parts.Add(country.Trim());
        }

        return string.Join(", ", parts);
    }
}
