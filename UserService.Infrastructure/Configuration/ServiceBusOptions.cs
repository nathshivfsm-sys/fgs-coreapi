namespace UserService.Infrastructure.Configuration;

public sealed class ServiceBusOptions
{
    public const string SectionName = "ServiceBus";

    public string? ConnectionString { get; set; }

    /// <summary>Topic or queue name used for integration events.</summary>
    public string InviteEventsPath { get; set; } = "integration-events";
}
