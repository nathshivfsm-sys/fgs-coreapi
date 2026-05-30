namespace Fgs.MultiTenancy;

public interface ITenantContextAccessor
{
    ITenantContext? Current { get; set; }
}
