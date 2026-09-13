using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Fgs.Credentials.Options;

namespace Fgs.Credentials;

public static class InternalServiceAuthorization
{
    /// <summary>
    /// Develop-environment shared S2S key. Replace before production.
    /// </summary>
    public const string DevelopInternalServiceKey = "fgs-internal-credential-distribution-key";

    public static bool IsAuthorized(string? providedKey, CredentialDistributionOptions options)
    {
        if (string.IsNullOrWhiteSpace(providedKey))
        {
            return false;
        }

        if (IsUsableConfiguredKey(options.InternalServiceKey)
            && FixedTimeEquals(providedKey, options.InternalServiceKey))
        {
            return true;
        }

        foreach (var key in options.AdditionalInternalServiceKeys)
        {
            if (IsUsableConfiguredKey(key) && FixedTimeEquals(providedKey, key))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsAuthorizedOrUserAuthenticated(
        string? providedKey,
        CredentialDistributionOptions options,
        ClaimsPrincipal? user) =>
        IsAuthorized(providedKey, options) || user?.Identity?.IsAuthenticated == true;

    public static bool HasUsableConfiguredKey(CredentialDistributionOptions options)
    {
        if (IsUsableConfiguredKey(options.InternalServiceKey))
        {
            return true;
        }

        return options.AdditionalInternalServiceKeys.Any(IsUsableConfiguredKey);
    }

    private static bool IsUsableConfiguredKey(string? key) =>
        !string.IsNullOrWhiteSpace(key);

    private static bool FixedTimeEquals(string provided, string expected)
    {
        var providedBytes = Encoding.UTF8.GetBytes(provided);
        var expectedBytes = Encoding.UTF8.GetBytes(expected);
        return CryptographicOperations.FixedTimeEquals(providedBytes, expectedBytes);
    }
}
