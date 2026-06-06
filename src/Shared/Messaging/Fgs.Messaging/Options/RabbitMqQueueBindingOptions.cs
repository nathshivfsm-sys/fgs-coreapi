namespace Fgs.Messaging.Options;

/// <summary>
/// Durable queue bound to a topic exchange before publishing or consuming.
/// </summary>
public sealed class RabbitMqQueueBindingOptions
{
    /// <summary>
    /// Topic exchange for this binding. When empty, <see cref="RabbitMqOptions.ExchangeName"/> is used.
    /// </summary>
    public string? ExchangeName { get; set; }

    public string QueueName { get; set; } = string.Empty;

    public string RoutingKey { get; set; } = string.Empty;

    public string? DeadLetterExchangeName { get; set; }

    public string? DeadLetterQueueName { get; set; }

    public string? DeadLetterRoutingKey { get; set; }
}
