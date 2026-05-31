namespace Fgs.Kernel.Entities;

/// <summary>
/// Marks entities scoped to a tenant only (no company dimension).
/// </summary>
public interface ITenantScoped
{
    long TenantId { get; }
}
