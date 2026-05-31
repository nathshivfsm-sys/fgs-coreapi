namespace Fgs.Security.Constants;

public static class FgsScopeConstants
{
    public const long PlatformTenantId = 0;

    public const long PlatformCompanyId = 0;

    public static bool IsPlatformScope(long tenantId, long companyId) =>
        tenantId == PlatformTenantId && companyId == PlatformCompanyId;
}
