namespace Fgs.Platform.Infrastructure.Options;

public sealed class RabbitMqQueueBindingOptions
{
    public string QueueName { get; set; } = string.Empty;

    public string RoutingKey { get; set; } = string.Empty;
}
