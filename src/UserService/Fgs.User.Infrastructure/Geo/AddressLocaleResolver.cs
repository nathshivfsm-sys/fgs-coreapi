using Fgs.User.Application.Abstractions.Geo;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Application.Features.Signup.DTOs;
using Fgs.User.Domain.Entities;
using Fgs.User.Infrastructure.Options;
using GeoTimeZone;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure.Geo;

public sealed class AddressLocaleResolver : IAddressLocaleResolver
{
    private readonly IRepository<GloCountry> _countryRepository;
    private readonly SignupLocaleOptions _options;

    public AddressLocaleResolver(
        IRepository<GloCountry> countryRepository,
        IOptions<SignupLocaleOptions> options)
    {
        _countryRepository = countryRepository;
        _options = options.Value;
    }

    public async Task<AddressLocale> ResolveAsync(
        SignupAddressDto address,
        CancellationToken cancellationToken = default)
    {
        var countryCode = AddressCountryNormalizer.ResolveCountryCode(address.Country, address.State);
        var timeZone = ResolveTimeZoneFromCoordinates(address)
            ?? (string.IsNullOrEmpty(countryCode)
                ? null
                : RegionalTimeZoneLookup.ResolveTimeZone(countryCode, address.State))
            ?? _options.DefaultTimeZone;

        var currency = await ResolveCurrencyAsync(countryCode, cancellationToken)
            ?? _options.DefaultCurrency;

        return new AddressLocale(timeZone, currency);
    }

    private static string? ResolveTimeZoneFromCoordinates(SignupAddressDto address)
    {
        if (address.Latitude is not decimal latitude || address.Longitude is not decimal longitude)
        {
            return null;
        }

        var result = TimeZoneLookup.GetTimeZone((double)latitude, (double)longitude);
        return string.IsNullOrWhiteSpace(result.Result) ? null : result.Result;
    }

    private async Task<string?> ResolveCurrencyAsync(string countryCode, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(countryCode))
        {
            return null;
        }

        var country = await _countryRepository.FirstOrDefaultAsync(
            c => c.CountryCode == countryCode && c.IsActive,
            cancellationToken);

        if (!string.IsNullOrWhiteSpace(country?.CurrencyCode))
        {
            return country.CurrencyCode.Trim().ToUpperInvariant();
        }

        return RegionalTimeZoneLookup.ResolveCurrencyFallback(countryCode);
    }
}
