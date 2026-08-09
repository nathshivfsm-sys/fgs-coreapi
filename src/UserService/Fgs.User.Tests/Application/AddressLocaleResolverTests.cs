using Fgs.Contracts.Signup;
using Fgs.User.Infrastructure.Common.Geo;
using Fgs.User.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;

namespace Fgs.User.Tests.Application;

public sealed class AddressLocaleResolverTests
{
    private static AddressLocaleResolver CreateResolver() =>
        new(Options.Create(new SignupLocaleOptions
        {
            DefaultTimeZone = "UTC",
            DefaultCurrency = "USD"
        }));

    [Fact]
    public async Task ResolveAsync_ForTexasAddress_ReturnsCentralTimeAndUsd()
    {
        var locale = await CreateResolver().ResolveAsync(
            new SignupAddressDto(
                AddressLine1: "100 Main St",
                AddressLine2: null,
                City: "Austin",
                State: "TX",
                PostalCode: "78701",
                Country: "US"));

        locale.TimeZoneId.Should().Be("America/Chicago");
        locale.CurrencyCode.Should().Be("USD");
    }

    [Fact]
    public async Task ResolveAsync_WithCoordinates_UsesGeoTimeZone()
    {
        var locale = await CreateResolver().ResolveAsync(
            new SignupAddressDto(
                AddressLine1: "1 Dr Carlton B Goodlett Pl",
                AddressLine2: null,
                City: "San Francisco",
                State: "CA",
                PostalCode: "94102",
                Country: "US",
                Latitude: 37.7793m,
                Longitude: -122.4193m));

        locale.TimeZoneId.Should().Be("America/Los_Angeles");
        locale.CurrencyCode.Should().Be("USD");
    }

    [Fact]
    public async Task ResolveAsync_WithoutCountry_InfersUsFromState()
    {
        var locale = await CreateResolver().ResolveAsync(
            new SignupAddressDto(
                AddressLine1: "1 Main",
                AddressLine2: null,
                City: "New York",
                State: "NY",
                PostalCode: "10001"));

        locale.TimeZoneId.Should().Be("America/New_York");
        locale.CurrencyCode.Should().Be("USD");
    }
}
