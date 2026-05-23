namespace Fgs.User.Application.TenantProvisioning;

/// <summary>
/// <see cref="GloSetupTenantStatus"/> identifiers (seeded in Initial_Migration_Seed.sql).
/// </summary>
public static class TenantStatusIds
{
    public const short Pending = 1;
    public const short Provisioning = 2;
    public const short Active = 3;
    public const short ProvisioningFailed = 4;
    public const short Suspended = 5;
    public const short Cancelled = 6;
}
