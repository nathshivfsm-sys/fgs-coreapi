using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Signup.DTOs;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Database;
using Fgs.User.Infrastructure.Geo;
using Fgs.User.Infrastructure.Options;
using Fgs.User.Infrastructure.Persistence;
using Microsoft.Extensions.Options;

namespace Fgs.User.Tests.Application;

public sealed class AddressLocaleResolverTests
{
    [Fact]
    public async Task ResolveAsync_ForTexasAddress_ReturnsCentralTimeAndUsd()
    {
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        SeedUnitedStates(context);
        var resolver = CreateResolver(context);

        var locale = await resolver.ResolveAsync(
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
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        SeedUnitedStates(context);
        var resolver = CreateResolver(context);

        var locale = await resolver.ResolveAsync(
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
        var context = await TestDbContextFactory.CreateAndInitializeAsync();
        SeedUnitedStates(context);
        var resolver = CreateResolver(context);

        var locale = await resolver.ResolveAsync(
            new SignupAddressDto(
                AddressLine1: "1 Main",
                AddressLine2: null,
                City: "New York",
                State: "NY",
                PostalCode: "10001"));

        locale.TimeZoneId.Should().Be("America/New_York");
        locale.CurrencyCode.Should().Be("USD");
    }

    private static void SeedUnitedStates(FgsUserDbContext context)
    {
        context.GloCountries.Add(new GloCountry
        {
            CountryCode = "US",
            CountryName = "United States",
            CurrencyCode = "USD",
            IsActive = true
        });
        context.SaveChanges();
    }

    private static AddressLocaleResolver CreateResolver(FgsUserDbContext context)
    {
        IUnitOfWork unitOfWork = new UnitOfWork(context);
        var options = Options.Create(new SignupLocaleOptions
        {
            DefaultTimeZone = "UTC",
            DefaultCurrency = "USD"
        });

        return new AddressLocaleResolver(unitOfWork.Repository<GloCountry>(), options);
    }
}
