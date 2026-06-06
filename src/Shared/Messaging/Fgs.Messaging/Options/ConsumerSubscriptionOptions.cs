namespace Fgs.Messaging.Options;

public sealed class ConsumerSubscriptionOptions
{
    public string QueueName { get; set; } = string.Empty;

    public string ExchangeName { get; set; } = string.Empty;

    public string RoutingKey { get; set; } = string.Empty;

    public string? DeadLetterExchangeName { get; set; }

    public string? DeadLetterQueueName { get; set; }

    public string? DeadLetterRoutingKey { get; set; }
}
