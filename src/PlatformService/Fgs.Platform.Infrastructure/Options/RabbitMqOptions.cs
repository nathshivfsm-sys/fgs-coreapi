namespace Fgs.Platform.Infrastructure.Options;

public sealed class RabbitMqOptions
{
    public const string SectionName = "RabbitMq";

    public string? ConnectionUri { get; set; }

    public string HostName { get; set; } = "localhost";

    public int Port { get; set; } = 5672;

    public string UserName { get; set; } = "guest";

    public string Password { get; set; } = "guest";

    public bool SslEnabled { get; set; }

    public string? SslServerName { get; set; }

    public int ConnectionTimeoutSeconds { get; set; } = 30;

    public bool SslCheckCertificateRevocation { get; set; }

    public string ExchangeName { get; set; } = "fgs.user";

    public bool EnsureQueuesOnStartup { get; set; } = true;

    public string NotificationQueueName { get; set; } = "fgs.platform.notifications";

    public string DeadLetterQueueName { get; set; } = "fgs.platform.notifications.dlq";

    public string DeadLetterExchangeName { get; set; } = "fgs.platform.dlx";

    public IList<RabbitMqQueueBindingOptions> QueueBindings { get; set; } = [];
}
