namespace Fgs.MultiTenancy;

public interface ITenantContext
{
    long TenantId { get; }

    long CompanyId { get; }

    bool IsResolved { get; }
}
