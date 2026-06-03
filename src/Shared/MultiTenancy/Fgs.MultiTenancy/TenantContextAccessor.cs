namespace Fgs.MultiTenancy;

public sealed class TenantContextAccessor : ITenantContextAccessor
{
    public ITenantContext? Current { get; set; }
}
