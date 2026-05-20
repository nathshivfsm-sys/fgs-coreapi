namespace Fgs.User.Infrastructure.Common.Options;

/// <summary>
/// Durable queue bound to <see cref="RabbitMqOptions.ExchangeName"/> before publishing.
/// </summary>
public sealed class RabbitMqQueueBindingOptions
{
    public string QueueName { get; set; } = string.Empty;

    public string RoutingKey { get; set; } = string.Empty;
}
