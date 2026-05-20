namespace Fgs.User.Application.Features.Signup;

public static class AddressCountryNormalizer
{
    private static readonly HashSet<string> UsStateCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "AL", "AK", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "HI", "ID", "IL", "IN", "IA", "KS", "KY",
        "LA", "ME", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "OH",
        "OK", "OR", "PA", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VA", "WA", "WV", "WI", "WY", "PR", "VI", "GU",
        "AS", "MP"
    };

    private static readonly HashSet<string> CaProvinceCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "AB", "BC", "MB", "NB", "NL", "NS", "NT", "NU", "ON", "PE", "QC", "SK", "YT"
    };

    public static string ResolveCountryCode(string? country, string stateProvince)
    {
        if (!string.IsNullOrWhiteSpace(country))
        {
            return NormalizeCountryName(country.Trim());
        }

        var region = stateProvince.Trim();
        if (UsStateCodes.Contains(region))
        {
            return "US";
        }

        if (CaProvinceCodes.Contains(region))
        {
            return "CA";
        }

        return string.Empty;
    }

    private static string NormalizeCountryName(string value)
    {
        if (value.Length == 2)
        {
            return value.ToUpperInvariant();
        }

        return value.ToUpperInvariant() switch
        {
            "USA" or "U.S." or "U.S.A." or "UNITED STATES" or "UNITED STATES OF AMERICA" => "US",
            "CANADA" => "CA",
            "UK" or "U.K." or "UNITED KINGDOM" or "GREAT BRITAIN" => "GB",
            "AUSTRALIA" => "AU",
            "MEXICO" or "MÉXICO" => "MX",
            "GERMANY" or "DEUTSCHLAND" => "DE",
            "FRANCE" => "FR",
            "INDIA" => "IN",
            "NEW ZEALAND" => "NZ",
            _ => value.Length <= 3 ? value.ToUpperInvariant() : string.Empty
        };
    }
}
