namespace Fgs.Messaging.Options;

public sealed class ConsumerOptions
{
    public const string SectionName = "Consumer";

    public bool Enabled { get; set; } = true;

    public ushort PrefetchCount { get; set; } = 10;

    public int MaxRetryAttempts { get; set; } = 5;

    public int InitialRetryDelaySeconds { get; set; } = 5;

    public IList<ConsumerSubscriptionOptions> Subscriptions { get; set; } = [];
}
