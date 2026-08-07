using System.Security.Claims;
using Fgs.Credentials.Options;

namespace Fgs.Credentials;

public static class InternalServiceAuthorization
{
    public static bool IsAuthorized(string? providedKey, CredentialDistributionOptions options)
    {
        if (string.IsNullOrWhiteSpace(providedKey))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(options.InternalServiceKey)
            && string.Equals(providedKey, options.InternalServiceKey, StringComparison.Ordinal))
        {
            return true;
        }

        foreach (var key in options.AdditionalInternalServiceKeys)
        {
            if (!string.IsNullOrWhiteSpace(key)
                && string.Equals(providedKey, key, StringComparison.Ordinal))
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
}
