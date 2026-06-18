using Fgs.User.Application.Abstractions.Identity;

namespace Fgs.User.Application.Features.Auth;

public static class AuthScopeValidation
{
    public static bool TryValidateHeadersAgainstProfile(
        long? headerTenantId,
        long? headerCompanyId,
        FgsUserProfile profile,
        out IReadOnlyList<string> errors)
    {
        var validationErrors = new List<string>();

        if (headerTenantId is null || headerCompanyId is null)
        {
            validationErrors.Add("X-Tenant-Id and X-Company-Id headers are required.");
        }
        else if (headerTenantId.Value != profile.TenantId || headerCompanyId.Value != profile.CompanyId)
        {
            validationErrors.Add("Tenant scope headers do not match the authenticated user.");
        }

        errors = validationErrors;
        return validationErrors.Count == 0;
    }
}
