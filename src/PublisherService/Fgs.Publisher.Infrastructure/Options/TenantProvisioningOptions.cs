namespace Fgs.Publisher.Infrastructure.Options;

public sealed class TenantProvisioningOptions
{
    public const string SectionName = "TenantProvisioning";

    public string TenantEventsExchangeName { get; set; } = "tenant.events";
}
