namespace Fgs.Contracts.Clients;

/// <summary>
/// Glo setup tenant status identifiers (seeded in SetupService global seed).
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
