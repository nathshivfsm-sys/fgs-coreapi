using Fgs.User.Application.Signup;

namespace Fgs.User.Application.Abstractions.Geo;

public interface IAddressLocaleResolver
{
    Task<AddressLocale> ResolveAsync(SignupAddressDto address, CancellationToken cancellationToken = default);
}

public sealed record AddressLocale(string TimeZoneId, string CurrencyCode);
