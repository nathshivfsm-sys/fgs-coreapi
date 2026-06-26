using Fgs.User.Application.Abstractions.Geo;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Application.Features.Signup.DTOs;
using Fgs.User.Infrastructure.Common.Options;
using GeoTimeZone;
using Microsoft.Extensions.Options;

namespace Fgs.User.Infrastructure.Common.Geo;

public sealed class AddressLocaleResolver : IAddressLocaleResolver
{
    private readonly SignupLocaleOptions _options;

    public AddressLocaleResolver(IOptions<SignupLocaleOptions> options)
    {
        _options = options.Value;
    }

    public Task<AddressLocale> ResolveAsync(
        SignupAddressDto address,
        CancellationToken cancellationToken = default)
    {
        var countryCode = AddressCountryNormalizer.ResolveCountryCode(address.Country, address.State);
        var timeZone = ResolveTimeZoneFromCoordinates(address)
            ?? (string.IsNullOrEmpty(countryCode)
                ? null
                : RegionalTimeZoneLookup.ResolveTimeZone(countryCode, address.State))
            ?? _options.DefaultTimeZone;

        var currency = ResolveCurrency(countryCode) ?? _options.DefaultCurrency;

        return Task.FromResult(new AddressLocale(timeZone, currency));
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

    private static string? ResolveCurrency(string? countryCode)
    {
        if (string.IsNullOrEmpty(countryCode))
        {
            return null;
        }

        return RegionalTimeZoneLookup.ResolveCurrencyFallback(countryCode);
    }
}
