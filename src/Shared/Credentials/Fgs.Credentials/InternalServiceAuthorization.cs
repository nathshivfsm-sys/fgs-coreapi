using System.Security.Claims;
using Fgs.Credentials.Options;

namespace Fgs.Credentials;

public static class InternalServiceAuthorization
{
    public static bool IsAuthorized(string? providedKey, CredentialDistributionOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.InternalServiceKey))
        {
            return false;
        }

        return string.Equals(providedKey, options.InternalServiceKey, StringComparison.Ordinal);
    }

    public static bool IsAuthorizedOrUserAuthenticated(
        string? providedKey,
        CredentialDistributionOptions options,
        ClaimsPrincipal? user) =>
        IsAuthorized(providedKey, options) || user?.Identity?.IsAuthenticated == true;
}
