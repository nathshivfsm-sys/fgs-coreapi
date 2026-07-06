using Fgs.MultiTenancy;

namespace Fgs.Asset.Infrastructure.Common;

internal static class AssetTenantScopeResolver
{
    internal static (long TenantId, long CompanyId) ResolveRequired(ITenantContextAccessor tenantContextAccessor)
    {
        if (tenantContextAccessor.Current is ITenantContext context)
        {
            return (context.TenantId, context.CompanyId);
        }

        throw new InvalidOperationException("Tenant context is not resolved.");
    }
}
