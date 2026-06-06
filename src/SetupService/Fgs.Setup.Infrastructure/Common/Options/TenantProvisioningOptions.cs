namespace Fgs.Setup.Infrastructure.Common.Options;

public sealed class TenantProvisioningOptions
{
    public const string SectionName = "TenantProvisioning";

    public int MaxRetryAttempts { get; set; } = 5;

    public int InitialRetryDelaySeconds { get; set; } = 5;

    public int SeedingBatchSize { get; set; } = 500;

    /// <summary>
    /// Optional full connection strings keyed by database name for cross-database seeding.
    /// When absent, the FgsUser base connection string is reused with <c>Database</c> replaced.
    /// </summary>
    public Dictionary<string, string> DatabaseConnectionStrings { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public string TenantEventsExchangeName { get; set; } = "tenant.events";
}
