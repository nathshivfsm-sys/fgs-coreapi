using System.Text;
using System.Text.Json;
using Fgs.User.Application.Abstractions.Provisioning;
using Fgs.User.Application.IntegrationEvents;
using Fgs.User.Infrastructure.Common.Options;
using Fgs.User.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Fgs.User.Infrastructure.Background;

public sealed class TenantProvisionConsumerService(
    IServiceScopeFactory scopeFactory,
    IOptions<RabbitMqOptions> rabbitOptions,
    IOptions<RabbitMqConsumerOptions> consumerOptions,
    RabbitMqTopologyService topologyService,
    ILogger<TenantProvisionConsumerService> logger) : BackgroundService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly RabbitMqOptions _rabbitOptions = rabbitOptions.Value;
    private readonly RabbitMqConsumerOptions _consumerOptions = consumerOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_consumerOptions.Enabled)
        {
            logger.LogInformation("Tenant provision consumer is disabled");
            return;
        }

        var connection = await CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await topologyService.EnsureTenantProvisioningTopologyAsync(channel, stoppingToken);
        await channel.BasicQosAsync(0, _consumerOptions.PrefetchCount, false, stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, args) =>
        {
            var deliveryTag = args.DeliveryTag;
            var correlationId = args.BasicProperties.CorrelationId ?? Guid.NewGuid().ToString();
            var retryCount = GetRetryCount(args.BasicProperties.Headers);

            try
            {
                var body = Encoding.UTF8.GetString(args.Body.ToArray());
                var request = JsonSerializer.Deserialize<TenantProvisionRequestedEvent>(body, JsonOptions)
                    ?? throw new InvalidOperationException("Invalid TenantProvisionRequested payload.");

                using var scope = scopeFactory.CreateScope();
                var orchestrator = scope.ServiceProvider.GetRequiredService<ITenantProvisioningOrchestrator>();
                await orchestrator.ProvisionAsync(request, stoppingToken);

                await channel.BasicAckAsync(deliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Tenant provision message failed (retry {RetryCount}, correlation {CorrelationId})",
                    retryCount,
                    correlationId);

                if (retryCount >= _consumerOptions.MaxRetryAttempts)
                {
                    await channel.BasicNackAsync(deliveryTag, false, false, stoppingToken);
                    logger.LogWarning(
                        "Message sent to DLQ after {MaxRetries} attempts, correlation {CorrelationId}",
                        _consumerOptions.MaxRetryAttempts,
                        correlationId);
                }
                else
                {
                    var delaySeconds = _consumerOptions.InitialRetryDelaySeconds * Math.Pow(2, retryCount);
                    logger.LogWarning(
                        "Requeueing message after {DelaySeconds}s (attempt {Attempt})",
                        delaySeconds,
                        retryCount + 1);
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds), stoppingToken);
                    await channel.BasicNackAsync(deliveryTag, false, true, stoppingToken);
                }
            }
        };

        await channel.BasicConsumeAsync(
            queue: _consumerOptions.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        logger.LogInformation(
            "Tenant provision consumer listening on queue {Queue}",
            _consumerOptions.QueueName);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            // shutdown
        }
    }

    private static int GetRetryCount(IDictionary<string, object?>? headers)
    {
        if (headers is null || !headers.TryGetValue("x-retry-count", out var value))
        {
            return 0;
        }

        return value switch
        {
            int i => i,
            long l => (int)l,
            byte[] bytes when int.TryParse(Encoding.UTF8.GetString(bytes), out var parsed) => parsed,
            _ => 0
        };
    }

    private async Task<IConnection> CreateConnectionAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            ClientProvidedName = "Fgs.User.TenantProvisionConsumer",
            AutomaticRecoveryEnabled = true
        };

        if (!string.IsNullOrWhiteSpace(_rabbitOptions.ConnectionUri))
        {
            factory.Uri = new Uri(_rabbitOptions.ConnectionUri);
        }
        else
        {
            factory.HostName = _rabbitOptions.HostName;
            factory.Port = _rabbitOptions.Port;
            factory.UserName = _rabbitOptions.UserName;
            factory.Password = _rabbitOptions.Password;
        }

        return await factory.CreateConnectionAsync(cancellationToken);
    }
}
