namespace Fgs.Contracts.Signup;

/// <summary>Structured company address for signup onboarding.</summary>
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
