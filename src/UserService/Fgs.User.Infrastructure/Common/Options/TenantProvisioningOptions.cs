namespace Fgs.User.Infrastructure.Common.Options;

public sealed class TenantProvisioningOptions
{
    public const string SectionName = "TenantProvisioning";

    public int MaxRetryAttempts { get; set; } = 5;

    public int InitialRetryDelaySeconds { get; set; } = 5;

    public int SeedingBatchSize { get; set; } = 500;

    public string TenantEventsExchangeName { get; set; } = "tenant.events";
}
