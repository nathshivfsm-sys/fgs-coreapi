using Fgs.User.Application.Features.Signup.DTOs;

using Fgs.User.Domain.Entities;

namespace Fgs.User.Application.Features.Signup;

public static class SignupLocationFactory
{
    public static FgsLocation CreateCompanyLocation(
        Guid locationId,
        long tenantId,
        long companyId,
        int masterEntityTypeId,
        SignupAddressDto address,
        DateTimeOffset createdOn)
    {
        var line1 = address.AddressLine1.Trim();
        var line2 = string.IsNullOrWhiteSpace(address.AddressLine2) ? null : address.AddressLine2.Trim();
        var city = address.City.Trim();
        var state = address.State.Trim();
        var postalCode = address.PostalCode.Trim();
        var county = string.IsNullOrWhiteSpace(address.County) ? null : address.County.Trim();
        var country = string.IsNullOrWhiteSpace(address.Country) ? null : address.Country.Trim();
        var placeId = string.IsNullOrWhiteSpace(address.PlaceId) ? null : address.PlaceId.Trim();

        return new FgsLocation
        {
            Id = locationId,
            TenantId = tenantId,
            CompanyId = companyId,
            MasterEntityTypeId = masterEntityTypeId,
            AddressLine1 = line1,
            AddressLine2 = line2,
            City = city,
            State = state,
            County = county,
            Country = country,
            PostalCode = postalCode,
            FormattedAddress = FormatAddress(line1, line2, city, state, postalCode, country),
            Latitude = address.Latitude,
            Longitude = address.Longitude,
            PlaceId = placeId,
            IsActive = true,
            CreatedOn = createdOn
        };
    }

    private static string FormatAddress(
        string line1,
        string? line2,
        string city,
        string state,
        string postalCode,
        string? country)
    {
        var parts = new List<string> { line1 };
        if (!string.IsNullOrWhiteSpace(line2))
        {
            parts.Add(line2);
        }

        parts.Add($"{city}, {state} {postalCode}");
        if (!string.IsNullOrWhiteSpace(country))
        {
            parts.Add(country);
        }

        return string.Join(", ", parts);
    }
}
