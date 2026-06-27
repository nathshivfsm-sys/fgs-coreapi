using Fgs.MultiTenancy;

namespace Fgs.Inventory.Infrastructure.Common;

internal static class InventoryTenantScopeResolver
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
