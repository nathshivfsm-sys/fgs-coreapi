namespace Fgs.MultiTenancy.Persistence;

/// <summary>
/// Design-time / migration accessor with no resolved tenant (filters inactive).
/// </summary>
public sealed class DesignTimeTenantContextAccessor : ITenantContextAccessor
{
    public ITenantContext? Current { get; set; }
}
