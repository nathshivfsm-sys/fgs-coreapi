namespace Fgs.MultiTenancy.Constants;

public static class TenantScopeConstants
{
    /// <summary>Platform-global scope when credentials or resources are not tenant/company specific.</summary>
    public const long PlatformTenantId = 0;

    public const long PlatformCompanyId = 0;

    public const string PlatformTenantCode = "platform";

    public static bool IsPlatformScope(long tenantId, long companyId) =>
        tenantId == PlatformTenantId && companyId == PlatformCompanyId;
}
