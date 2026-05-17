namespace Fgs.User.Infrastructure.Geo;

internal static class RegionalTimeZoneLookup
{
    private static readonly IReadOnlyDictionary<string, string> UsStateTimeZones =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["AL"] = "America/Chicago",
            ["AK"] = "America/Anchorage",
            ["AZ"] = "America/Phoenix",
            ["AR"] = "America/Chicago",
            ["CA"] = "America/Los_Angeles",
            ["CO"] = "America/Denver",
            ["CT"] = "America/New_York",
            ["DE"] = "America/New_York",
            ["DC"] = "America/New_York",
            ["FL"] = "America/New_York",
            ["GA"] = "America/New_York",
            ["HI"] = "Pacific/Honolulu",
            ["ID"] = "America/Boise",
            ["IL"] = "America/Chicago",
            ["IN"] = "America/Indiana/Indianapolis",
            ["IA"] = "America/Chicago",
            ["KS"] = "America/Chicago",
            ["KY"] = "America/New_York",
            ["LA"] = "America/Chicago",
            ["ME"] = "America/New_York",
            ["MD"] = "America/New_York",
            ["MA"] = "America/New_York",
            ["MI"] = "America/Detroit",
            ["MN"] = "America/Chicago",
            ["MS"] = "America/Chicago",
            ["MO"] = "America/Chicago",
            ["MT"] = "America/Denver",
            ["NE"] = "America/Chicago",
            ["NV"] = "America/Los_Angeles",
            ["NH"] = "America/New_York",
            ["NJ"] = "America/New_York",
            ["NM"] = "America/Denver",
            ["NY"] = "America/New_York",
            ["NC"] = "America/New_York",
            ["ND"] = "America/Chicago",
            ["OH"] = "America/New_York",
            ["OK"] = "America/Chicago",
            ["OR"] = "America/Los_Angeles",
            ["PA"] = "America/New_York",
            ["RI"] = "America/New_York",
            ["SC"] = "America/New_York",
            ["SD"] = "America/Chicago",
            ["TN"] = "America/Chicago",
            ["TX"] = "America/Chicago",
            ["UT"] = "America/Denver",
            ["VT"] = "America/New_York",
            ["VA"] = "America/New_York",
            ["WA"] = "America/Los_Angeles",
            ["WV"] = "America/New_York",
            ["WI"] = "America/Chicago",
            ["WY"] = "America/Denver",
            ["PR"] = "America/Puerto_Rico",
            ["VI"] = "America/Virgin",
            ["GU"] = "Pacific/Guam",
            ["AS"] = "Pacific/Pago_Pago",
            ["MP"] = "Pacific/Guam"
        };

    private static readonly IReadOnlyDictionary<string, string> CaProvinceTimeZones =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["AB"] = "America/Edmonton",
            ["BC"] = "America/Vancouver",
            ["MB"] = "America/Winnipeg",
            ["NB"] = "America/Moncton",
            ["NL"] = "America/St_Johns",
            ["NS"] = "America/Halifax",
            ["NT"] = "America/Yellowknife",
            ["NU"] = "America/Iqaluit",
            ["ON"] = "America/Toronto",
            ["PE"] = "America/Halifax",
            ["QC"] = "America/Toronto",
            ["SK"] = "America/Regina",
            ["YT"] = "America/Whitehorse"
        };

    private static readonly IReadOnlyDictionary<string, string> CountryDefaultTimeZones =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["US"] = "America/Chicago",
            ["CA"] = "America/Toronto",
            ["GB"] = "Europe/London",
            ["AU"] = "Australia/Sydney",
            ["MX"] = "America/Mexico_City",
            ["DE"] = "Europe/Berlin",
            ["FR"] = "Europe/Paris",
            ["IN"] = "Asia/Kolkata",
            ["NZ"] = "Pacific/Auckland",
            ["IE"] = "Europe/Dublin",
            ["JP"] = "Asia/Tokyo",
            ["BR"] = "America/Sao_Paulo"
        };

    private static readonly IReadOnlyDictionary<string, string> CountryCurrencyFallbacks =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["US"] = "USD",
            ["CA"] = "CAD",
            ["GB"] = "GBP",
            ["AU"] = "AUD",
            ["MX"] = "MXN",
            ["DE"] = "EUR",
            ["FR"] = "EUR",
            ["IN"] = "INR",
            ["NZ"] = "NZD",
            ["IE"] = "EUR",
            ["JP"] = "JPY",
            ["BR"] = "BRL"
        };

    public static string? ResolveTimeZone(string countryCode, string stateProvince)
    {
        var region = stateProvince.Trim();
        if (countryCode.Equals("US", StringComparison.OrdinalIgnoreCase)
            && UsStateTimeZones.TryGetValue(region, out var usZone))
        {
            return usZone;
        }

        if (countryCode.Equals("CA", StringComparison.OrdinalIgnoreCase)
            && CaProvinceTimeZones.TryGetValue(region, out var caZone))
        {
            return caZone;
        }

        return CountryDefaultTimeZones.TryGetValue(countryCode, out var countryZone)
            ? countryZone
            : null;
    }

    public static string? ResolveCurrencyFallback(string countryCode) =>
        CountryCurrencyFallbacks.TryGetValue(countryCode, out var currency) ? currency : null;
}
