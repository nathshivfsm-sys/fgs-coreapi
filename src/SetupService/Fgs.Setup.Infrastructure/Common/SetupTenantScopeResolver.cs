using Fgs.MultiTenancy;

namespace Fgs.Setup.Infrastructure.Common;

internal static class SetupTenantScopeResolver
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
