namespace Fgs.Setup.Infrastructure.Common.Options;

public sealed class RabbitMqConsumerOptions
{
    public const string SectionName = "RabbitMq:Consumers:TenantProvision";

    public bool Enabled { get; set; } = true;

    public string QueueName { get; set; } = "Fgs.Setup.tenant.provision";

    public string RoutingKey { get; set; } = "tenant.provision.requested";

    public string ExchangeName { get; set; } = "tenant.events";

    public string DeadLetterExchangeName { get; set; } = "tenant.events.dlx";

    public string DeadLetterQueueName { get; set; } = "Fgs.Setup.tenant.provision.dlq";

    public string DeadLetterRoutingKey { get; set; } = "tenant.provision.dlq";

    public ushort PrefetchCount { get; set; } = 10;

    public int MaxRetryAttempts { get; set; } = 5;

    public int InitialRetryDelaySeconds { get; set; } = 5;
}
