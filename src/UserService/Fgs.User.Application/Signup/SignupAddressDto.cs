namespace Fgs.User.Application.Signup;

/// <summary>
/// Structured company address mapped to <see cref="Fgs.User.Domain.Entities.FgsLocation"/> columns.
/// </summary>
public sealed record SignupAddressDto(
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostalCode,
    string? County = null,
    string? Country = null,
    string? PlaceId = null,
    decimal? Latitude = null,
    decimal? Longitude = null);
