namespace Fgs.MultiTenancy;

public sealed class TenantContext : ITenantContext
{
    public long TenantId { get; init; }

    public long CompanyId { get; init; }

    public bool IsResolved { get; init; }
}
